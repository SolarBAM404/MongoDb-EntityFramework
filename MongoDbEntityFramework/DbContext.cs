using MongoDB.Driver;
using MongoDbEntityFramework.Settings;

namespace MongoDbEntityFramework;

public class DbContext
{
    private IMongoDatabase? _database;
    
    public DbContext()
    {
    }
    
    public DbContext(DbSettings settings)
    {
        Initialize(settings);
    }

    public void Initialize(DbSettings settings)
    {
        if (settings == null)
            throw new ArgumentNullException(nameof(settings), "Database settings cannot be null.");
        
        MongoClient client = new MongoClient(settings.ConnString);
        _database = client.GetDatabase(settings.DatabaseName);
    }
    
    public IMongoCollection<TEntity> GetCollection<TEntity>()
        where TEntity : class
    {
        if (_database == null)
            throw new InvalidOperationException("Database is not initialized. Call Initialize() with valid settings before accessing collections.");
        
        return _database.GetCollection<TEntity>(typeof(TEntity).Name);
    }
}