using MongoDB.Driver;

namespace Shared.Implementations.Mongo;

public class MongoContext
{
    public IMongoDatabase Database { get; }
    public readonly string CollectionName;
    public MongoContext(string connection, string databaseName, string collection)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        if (databaseName == null)
        {
            throw new ArgumentNullException(nameof(databaseName));
        }

        this.CollectionName = collection ?? throw new ArgumentNullException(nameof(collection));

        var client = new MongoClient(connection);
        Database = client.GetDatabase(databaseName);
    }
}