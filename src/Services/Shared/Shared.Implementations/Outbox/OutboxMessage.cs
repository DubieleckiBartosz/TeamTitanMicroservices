using Shared.Implementations.Types;

namespace Shared.Implementations.Outbox;

public class OutboxMessage : IIdentifier
{
    internal OutboxMessage()
    {
    }

    public OutboxMessage(string type, string data, string? queueKey = null)
    {
        Id = Guid.NewGuid();
        Type = type;
        Data = data;
        QueueKey = queueKey;
        Created = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public DateTime Created { get; private set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public string? QueueKey { get; set; }
    public DateTime? Processed { get; set; }
}