using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Implementations.Types;

namespace Shared.Implementations.Snapshot;

public class SnapshotState : IIdentifier
{
    public SnapshotState(Guid aggregateId, long currentVersion, string snapshotType, string snapshotData)
    {
        Id = Guid.NewGuid();
        AggregateId = aggregateId;
        CurrentVersion = currentVersion;
        SnapshotType = snapshotType;
        SnapshotData = snapshotData;
        Created = DateTime.UtcNow;
    }

    private SnapshotState(Guid id, Guid aggregateId, 
        long currentVersion, string snapshotType, string snapshotData,
        DateTime created)
    {
        Id = id;
        AggregateId = aggregateId;
        CurrentVersion = currentVersion;
        SnapshotType = snapshotType;
        SnapshotData = snapshotData;
        Created = created;
    }

    public SnapshotState()
    {
    }

    [BsonId]
    [BsonElement("_id")]
    public Guid Id { get; }
    public Guid AggregateId { get; set; }
    public long CurrentVersion { get; set; }
    public string SnapshotType { get; set; }
    public string SnapshotData { get; set; }
    public DateTime Created { get; }

    public static SnapshotState? Deserialize(BsonDocument? doc)
    {
        if (doc == null)
        {
            return null;
        }

        var id = Guid.Parse(doc["_id"].AsString);
        var aggregateId = Guid.Parse(doc[nameof(AggregateId)].AsString);
        var type = doc[nameof(SnapshotType)].AsString;
        var data = doc[nameof(SnapshotData)].AsString; 
        var version = doc[nameof(CurrentVersion)].AsInt64;
        var created = doc[nameof(Created)].ToUniversalTime(); 

        var streamState = new SnapshotState(id, aggregateId, version, type, data, created);

        return streamState;
    }
}