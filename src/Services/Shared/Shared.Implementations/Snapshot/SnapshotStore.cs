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

        var indexOptions = new CreateIndexOptions {Unique = true};
        var indexModel = new CreateIndexModel<BsonDocument>(indexDefinition, indexOptions);

        await collection.Indexes.CreateOneAsync(indexModel);

        var newSnapshot = new BsonDocument
        {
            {"Id", state.Id},
            {"AggregateId", state.AggregateId},
            {"CurrentVersion", state.CurrentVersion},
            {"SnapshotData", state.SnapshotData},
            {"SnapshotType", state.SnapshotType},
            {"Created", state.Created}
        };

        await collection.InsertOneAsync(newSnapshot);
    }

    public async Task<SnapshotState?> GetLastSnapshotAsync(Guid aggregateId)
    {
        var snapshot = await _database.GetCollection<SnapshotState>(CollectionName)
            .Find(x => x.AggregateId == aggregateId)
            .SortByDescending(x => x.CurrentVersion)
            .FirstOrDefaultAsync();

        return snapshot;
    }
}