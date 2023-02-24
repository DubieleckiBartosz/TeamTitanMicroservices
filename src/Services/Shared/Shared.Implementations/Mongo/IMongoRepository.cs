using System.Linq.Expressions;
using MongoDB.Driver;
using Shared.Implementations.Types;

namespace Shared.Implementations.Mongo;

public interface IMongoRepository<T> where T : IIdentifier
{
    IMongoDatabase GetDatabase();
    IMongoCollection<T> GetCollection(string collectionName);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate, IMongoCollection<T> collection);
    Task AddAsync(T entity, IMongoCollection<T> collection);
}