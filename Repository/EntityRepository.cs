using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbEntityFramework.Models;
using MongoDbEntityFramework.Settings;

namespace MongoDbEntityFramework.Repository;

/// <summary>
/// Class for the Entity Repository.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class EntityRepository<TEntity> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()
{

    private DbSettings _settings;
    private readonly IMongoCollection<TEntity> _collection;

    public EntityRepository(DbSettings settings)
    {
        _settings = settings;

        MongoClient client = new(_settings.ConnString);
        IMongoDatabase? database = client.GetDatabase(_settings.DatabaseName);

        _collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
    }

    public bool Insert(TEntity entity)
    {
        entity.Id = ObjectId.GenerateNewId();
        Task? task = _collection.InsertOneAsync(entity);
        task.Wait();
        return task.IsCompleted;
    }

    public bool Update(TEntity entity)
    {
        if (entity.Id == ObjectId.Empty)
            return Insert(entity);

        return _collection.ReplaceOne(x => x.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = true })
            .ModifiedCount > 0;
    }

    public bool Delete(TEntity entity)
    {
        return _collection.DeleteOne(x => x.Id == entity.Id).DeletedCount > 0;
    }

    public long Count()
    {
        return _collection.CountDocuments(new BsonDocument());
    }

    public List<TEntity> GetAll()
    {
        return _collection.AsQueryable().ToList();
    }

    public IList<TEntity> SearchFor(Expression<Func<TEntity, bool>> expression)
    {
        return _collection.AsQueryable().Where(expression).ToList();
    }

    public async Task<TEntity?> GetSingle(ObjectId id)
    {
        IAsyncCursor<TEntity>? obj = await _collection.FindAsync(x => x.Id == id);
        return obj.First();
    }
}