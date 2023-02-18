using System.Linq.Expressions;
using MongoDB.Driver;
using Shared.Implementations.Types;

namespace Shared.Implementations.Mongo;

public interface IMongoRepository<T> where T : IIdentifier
{
    IMongoCollection<T> Collection { get; }
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
}