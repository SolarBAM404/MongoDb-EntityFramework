using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDbEntityFramework.Models;

namespace MongoDbEntityFramework.Repository;

/// <summary>
/// Interface for the Entity Repository.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()
{
    /// <summary>
    /// Inserts the specified entity.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>true if the operation was successful; otherwise, false.</returns>
    bool Insert(TEntity entity);

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>true if the operation was successful; otherwise, false.</returns>
    bool Update(TEntity entity);

    /// <summary>
    /// Deletes the specified entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>true if the operation was successful; otherwise, false.</returns>
    bool Delete(TEntity entity);

    /// <summary>
    /// Counts the total number of entities.
    /// </summary>
    /// <returns>The total number of entities.</returns>
    long Count();

    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <returns>A list of all entities.</returns>
    List<TEntity> GetAll();

    /// <summary>
    /// Searches for entities that match the specified expression.
    /// </summary>
    /// <param name="expression">The expression to match.</param>
    /// <returns>A list of entities that match the specified expression.</returns>
    IList<TEntity> SearchFor(Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// Gets a single entity by its id.
    /// </summary>
    /// <param name="id">The id of the entity to get.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<TEntity?> GetSingle(ObjectId id);
}