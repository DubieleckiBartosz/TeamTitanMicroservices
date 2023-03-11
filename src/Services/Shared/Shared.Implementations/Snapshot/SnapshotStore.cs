using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Implementations.Mongo;

namespace Shared.Implementations.Snapshot;

public class SnapshotStore : ISnapshotStore
{
    private const string CollectionName = "Snapshots";
    private readonly IMongoRepository<SnapshotState> _mongoRepository;
    private readonly IMongoDatabase _database;

    public SnapshotStore(IMongoRepository<SnapshotState> mongoRepository)
    {
        _mongoRepository = mongoRepository ?? throw new ArgumentNullException(nameof(mongoRepository));
        _database = _mongoRepository.GetDatabase();
    }

    public async Task AddAsync(SnapshotState state)
    {
        var collection = _database.GetCollection<BsonDocument>(CollectionName);

        var indexDefinition = Builders<BsonDocument>.IndexKeys.Combine(
            Builders<BsonDocument>.IndexKeys.Ascending("AggregateId"),
            Builders<BsonDocument>.IndexKeys.Ascending("CurrentVersion")
        );

        var indexOptions = new CreateIndexOptions() { Unique = false };

        var indexModel = new CreateIndexModel<BsonDocument>(indexDefinition, indexOptions);

        await collection.Indexes.CreateOneAsync(indexModel);

        var newSnapshot = new BsonDocument
        {
            {"_id", state.Id.ToString()},
            {"AggregateId", state.AggregateId.ToString()},
            {"CurrentVersion", state.CurrentVersion},
            {"SnapshotData", state.SnapshotData},
            {"SnapshotType", state.SnapshotType},
            {"Created", state.Created}
        };

        await collection.InsertOneAsync(newSnapshot);
    } 

    public async Task<SnapshotState?> GetLastSnapshotAsync(Guid aggregateId)
    {
        var filterBuilder = Builders<BsonDocument>.Filter;
        var filter = filterBuilder.Eq("AggregateId", aggregateId.ToString());
        var sort = Builders<BsonDocument>.Sort.Descending("CurrentVersion");

        var bson = await _database.GetCollection<BsonDocument>(CollectionName)
            .Find(filter)
            .Sort(sort)
            .FirstOrDefaultAsync();

        var response = SnapshotState.Deserialize(bson);
        return response;
    }
}