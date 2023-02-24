using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shared.Implementations.Types;

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

    public StreamState()
    {
    }

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; private set; }
    public Guid StreamId { get; set; }
    public string EventType { get; set; }
    public string StreamType { get; set; }
    public string StreamData { get; set; }
    public int Version { get; set; } = 0;
    public DateTime Created { get; private set; }
}