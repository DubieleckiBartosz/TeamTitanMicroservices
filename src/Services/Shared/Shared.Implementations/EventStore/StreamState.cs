using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Implementations.Types;
using System.Security.Cryptography;

namespace Shared.Implementations.EventStore;

public class StreamState : IIdentifier
{
    public StreamState(Guid streamId, string eventType, string streamType, string streamData)
    {
        Id = Guid.NewGuid(); 
        StreamId = streamId;
        EventType = eventType;
        StreamType = streamType;
        StreamData = streamData;
        Version = 0;
        Created = DateTime.UtcNow;
    }

    private StreamState(Guid id, Guid streamId, string eventType, string streamType, string streamData, long version,
        DateTime created)
    {
        Id = id;
        StreamId = streamId;
        EventType = eventType;
        StreamType = streamType;
        StreamData = streamData;
        Version = version;
        Created = created;
    }

    public StreamState()
    {
    }

    [BsonId]
    [BsonElement("_id")]
    public Guid Id { get; } 
    public Guid StreamId { get; set; }
    public string EventType { get; set; }
    public string StreamType { get; set; }
    public string StreamData { get; set; }
    public long Version { get; set; } = 0;
    public DateTime Created { get; }

    public static StreamState Deserialize(BsonDocument doc)
    {
        // Get the values of the BsonDocument fields
        var id = Guid.Parse(doc["_id"].AsString);
        var streamId = Guid.Parse(doc[nameof(StreamId)].AsString);
        var streamData = doc[nameof(StreamData)].AsString;
        var streamType = doc[nameof(StreamType)].AsString;
        var eventType = doc[nameof(EventType)].AsString;
        var version = doc[nameof(Version)].AsInt64;
        var created = doc[nameof(Created)].ToUniversalTime();

        // Create a new StreamState object with the deserialized values
        var streamState = new StreamState(id, streamId, eventType, streamType, streamData, version, created);

        return streamState;
    }
} 