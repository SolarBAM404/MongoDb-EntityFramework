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

    private readonly IMongoCollection<TEntity> _collection;

    public EntityRepository(DbContext context)
    {
        _collection = context.GetCollection<TEntity>();
    }
    
    public EntityRepository(DbSettings settings)
    {
        MongoClient client = new(settings.ConnString);
        IMongoDatabase? database = client.GetDatabase(settings.DatabaseName);

        _collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
    }

    /// <summary>
    /// Inserts a new entity into the MongoDB collection.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>True if the insert operation completed; otherwise, false.</returns>
    public bool Insert(TEntity entity)
    {
        entity.Id = ObjectId.GenerateNewId();
        Task? task = _collection.InsertOneAsync(entity);
        task.Wait();
        return task.IsCompleted;
    }

    /// <summary>
    /// Updates an existing entity in the collection or inserts it if it does not exist.
    /// </summary>
    /// <param name="entity">The entity to update or insert.</param>
    /// <returns>True if the entity was updated; otherwise, false.</returns>
    public bool Update(TEntity entity)
    {
        if (entity.Id == ObjectId.Empty)
            return Insert(entity);

        return _collection.ReplaceOne(x => x.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = true })
            .ModifiedCount > 0;
    }

    /// <summary>
    /// Updates an existing entity in the collection or inserts it if it does not exist.
    /// </summary>
    /// <param name="entity">The entity to update or insert.</param>
    /// <returns>True if the entity was updated; otherwise, false.</returns>
    public bool Delete(TEntity entity)
    {
        return _collection.DeleteOne(x => x.Id == entity.Id).DeletedCount > 0;
    }

    /// <summary>
    /// Gets the total number of documents in the collection.
    /// </summary>
    /// <returns>The count of documents.</returns>
    public long Count()
    {
        return _collection.CountDocuments(new BsonDocument());
    }

    /// <summary>
    /// Retrieves all entities from the collection.
    /// </summary>
    /// <returns>A list of all entities.</returns>
    public List<TEntity> GetAll()
    {
        return _collection.AsQueryable().ToList();
    }

    /// <summary>
    /// Searches for entities matching the specified expression.
    /// </summary>
    /// <param name="expression">The filter expression.</param>
    /// <returns>A list of matching entities.</returns>
    public IList<TEntity> SearchFor(Expression<Func<TEntity, bool>> expression)
    {
        return _collection.AsQueryable().Where(expression).ToList();
    }

    /// <summary>
    /// Retrieves a single entity by its ObjectId.
    /// </summary>
    /// <param name="id">The ObjectId of the entity.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public async Task<TEntity?> GetSingle(ObjectId id)
    {
        IAsyncCursor<TEntity>? obj = await _collection.FindAsync(x => x.Id == id);
        return obj.First();
    }
}