using MongoDB.Driver;
using Shared.Implementations.Types;
using System.Linq.Expressions;

namespace Shared.Implementations.Mongo;

public class MongoRepository<T> : IMongoRepository<T> where T : IIdentifier
{ 
    private readonly IMongoDatabase _mongoDatabase;
    public MongoRepository(MongoContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        _mongoDatabase = context.Database; 
    }

    public IMongoDatabase GetDatabase() => _mongoDatabase;
    public IMongoCollection<T> GetCollection(string collectionName)
    {
        if (collectionName == null)
        {
            throw new ArgumentNullException(nameof(collectionName));
        }

        return _mongoDatabase.GetCollection<T>(collectionName);
    } 

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, IMongoCollection<T> collection)
    {
        return await collection.Find(predicate).SingleOrDefaultAsync();
    }

    public async Task AddAsync(T entity, IMongoCollection<T> collection)
    {
        await collection.InsertOneAsync(entity);
    }
}