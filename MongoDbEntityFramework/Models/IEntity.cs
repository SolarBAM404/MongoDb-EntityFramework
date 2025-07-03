using MongoDB.Bson;

namespace MongoDbEntityFramework.Models;

public interface IEntity
{

    ObjectId Id { get; set; }

}