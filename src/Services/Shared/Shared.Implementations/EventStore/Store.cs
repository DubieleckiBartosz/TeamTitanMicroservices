using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Mongo;

namespace Shared.Implementations.EventStore;

public class Store : IStore
{
    private const string CollectionName = "Events";
    private readonly IMongoRepository<StreamState> _mongoRepository;
    private readonly IMongoDatabase _database;
    public Store(IMongoRepository<StreamState> mongoRepository)
    {
        _mongoRepository = mongoRepository ?? throw new ArgumentNullException(nameof(mongoRepository));
        _database = _mongoRepository.GetDatabase();
    }

    public async Task AddAsync(StreamState stream, long? expectedVersion)
    {
        var eventsCollection = _database.GetCollection<BsonDocument>(CollectionName);

        var indexDefinition = Builders<BsonDocument>.IndexKeys.Combine(
            Builders<BsonDocument>.IndexKeys.Ascending("StreamId"),
            Builders<BsonDocument>.IndexKeys.Ascending("Version")
        );

        var indexOptions = new CreateIndexOptions() { Unique = false };
        var indexModel = new CreateIndexModel<BsonDocument>(indexDefinition, indexOptions);

        await eventsCollection.Indexes.CreateOneAsync(indexModel);
           
        var streamId = stream.StreamId;
        var streamType = stream.StreamType;

        var versionFailed = false;
        var streamFilter = Builders<BsonDocument>.Filter.Eq("StreamId", streamId);
        var streamVersion = eventsCollection.Find(streamFilter).FirstOrDefault()?.GetValue("Version", 0L).ToInt64();

        if (streamVersion.HasValue && (expectedVersion.HasValue && streamVersion != expectedVersion.Value))
        {
            versionFailed = true;
        }
        else
        {
            var newVersion = streamVersion.HasValue ? streamVersion.Value + 1 : 1;
            var eventDocument = new BsonDocument
            {
                {"_id", stream.Id.ToString()},
                {"StreamId", streamId.ToString()},
                {"StreamData", stream.StreamData},
                {"StreamType", streamType},
                {"EventType", stream.EventType},
                {"Version", newVersion},
                {"Created", DateTime.UtcNow}
            };
             
            await eventsCollection.InsertOneAsync(eventDocument);
        }

        if (versionFailed)
        {
            throw new EventException("Expected version did not match the stream version!", "Bad Stream Version");
        }
    }


    //public async Task<IReadOnlyList<StreamState>?> GetEventsAsync(Guid aggregateId, long? startVersion = null, long? version = null, DateTime? createdUtc = null)
    //{
    //    var filterBuilder = Builders<StreamState>.Filter;
    //    var filter = filterBuilder.Eq(e => e.StreamId, aggregateId);

    //    if (createdUtc.HasValue)
    //    {
    //        filter &= filterBuilder.Lte(e => e.Created, createdUtc.Value);
    //    }

    //    if (version.HasValue)
    //    {
    //        filter &= filterBuilder.Eq(e => e.Version, version.Value);
    //    }

    //    if (startVersion.HasValue)
    //    {
    //        filter &= filterBuilder.Gte(e => e.Version, startVersion.Value);
    //    }

    //    var sort = Builders<StreamState>.Sort.Ascending(e => e.Version);


    //    var events = await _database.GetCollection<StreamState>(CollectionName)
    //        .Find(filter)
    //        .Sort(sort)
    //        .ToListAsync();

    //    return events?.AsReadOnly();
    //}

    public async Task<IReadOnlyList<StreamState>?> GetEventsAsync(Guid aggregateId, long? startVersion = null, long? version = null, DateTime? createdUtc = null)
    {
        var filterBuilder = Builders<BsonDocument>.Filter;
        var filter = filterBuilder.Eq("StreamId", aggregateId.ToString());

        if (createdUtc.HasValue)
        {
            filter &= filterBuilder.Lte("Created", createdUtc.Value);
        }

        if (version.HasValue)
        {
            filter &= filterBuilder.Eq("Version", version.Value);
        }

        if (startVersion.HasValue)
        {
            filter &= filterBuilder.Gte("Version", startVersion.Value);
        }

        var sort = Builders<BsonDocument>.Sort.Ascending("Version");


        var bsonEvents = await _database.GetCollection<BsonDocument>(CollectionName)
            .Find(filter)
            .Sort(sort)
            .ToListAsync();

        var response = new List<StreamState>();
        foreach (var @event in bsonEvents)
        {
            var result = StreamState.Deserialize(@event);
            response.Add(result);
        }
       

        return response?.AsReadOnly();
    } 
}