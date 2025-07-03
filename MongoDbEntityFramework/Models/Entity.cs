using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbEntityFramework.Models;

public abstract class Entity : IEntity
{

    [BsonId]
    public ObjectId Id { get; set; }

}