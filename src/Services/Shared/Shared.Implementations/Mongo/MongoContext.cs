using MongoDB.Driver;

namespace Shared.Implementations.Mongo;

public class MongoContext
{
    public IMongoDatabase Database { get; } 
    public MongoContext(string connection, string databaseName)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        if (databaseName == null)
        {
            throw new ArgumentNullException(nameof(databaseName));
        } 

        var client = new MongoClient(connection);
        Database = client.GetDatabase(databaseName);
    }
}