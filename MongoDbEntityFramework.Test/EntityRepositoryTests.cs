using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbEntityFramework.Models;
using MongoDbEntityFramework.Repository;
using MongoDbEntityFramework.Settings;
using Moq;

namespace MongoDbEntityFramework.Test;

public class SampleEntity : IEntity
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
}

[TestFixture]
public class EntityRepositoryTests
{
    private DbContext _context;
    private Mock<IMongoCollection<SampleEntity>> _mockCollection;
    private EntityRepository<SampleEntity> _repository;

    [SetUp]
    public void Setup()
    {
        _mockCollection = new Mock<IMongoCollection<SampleEntity>>();
        DbSettings dbSettings = new DbSettings("mongodb://root:example@localhost:27017", "TestDatabase");
        _context = new DbContext(dbSettings);
        _repository = new EntityRepository<SampleEntity>(_context);
    }

    [Test]
    public void Insert_ShouldReturnTrue_WhenEntityInserted()
    {
        var entity = new SampleEntity();
        var result = _repository.Insert(entity);
        Assert.That(result);
    }

    [Test]
    public void Update_ShouldReturnTrue_WhenEntityUpdated()
    {
        var entity = new SampleEntity();
        var result = _repository.Update(entity);
        Assert.That(result);
    }

    [Test]
    public void Delete_ShouldReturnTrue_WhenEntityDeleted()
    {
        var entity = new SampleEntity();
        _repository.Insert(entity);
        var result = _repository.Delete(entity);
        Assert.That(result);
    }

    [Test]
    public void Count_ShouldReturnNumberOfDocuments()
    {
        var result = _repository.Count();
        Assert.That(result >= 0);
    }

    [Test]
    public void GetAll_ShouldReturnListOfEntities()
    {
        var result = _repository.GetAll();
        Assert.That(result is List<SampleEntity>);
    }

    [Test]
    public void SearchFor_ShouldReturnMatchingEntities()
    {
        Expression<Func<SampleEntity, bool>> filter = e => e.Name == "Test";
        var result = _repository.SearchFor(filter);
        Assert.That(result is IList<SampleEntity>);
    }

    [Test]
    public async Task GetSingle_ShouldReturnEntityOrNull()
    {
        var entity = new SampleEntity { Id = ObjectId.GenerateNewId() };
        _repository.Insert(entity);
        var result = await _repository.GetSingle(entity.Id);
        Assert.That(result == null || result is SampleEntity);
    }
}