using MongoDB.Bson.Serialization.Attributes;
using Shared.Implementations.Types;

namespace Shared.Implementations.Outbox;

public class OutboxMessage : IIdentifier
{
    public OutboxMessage()
    {
        Id = Guid.NewGuid();
        Created = DateTime.UtcNow;
    }

    public OutboxMessage(string type, string data, string? queueKey = null) : this()
    { 
        Type = type;
        Data = data;
        QueueKey = queueKey; 
        IsProcessed = false;
    }

    [BsonId] 
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public string? QueueKey { get; set; }
    public bool IsProcessed { get; set; }
    public DateTime? Processed { get; set; } = null;
}