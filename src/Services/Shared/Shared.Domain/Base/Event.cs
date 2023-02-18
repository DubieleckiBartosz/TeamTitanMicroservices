using Shared.Domain.Abstractions;

namespace Shared.Domain.Base;

public class Event : IEvent
{
    public Event()
    {
        CreatedUtc = DateTime.UtcNow;
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; }
    public DateTime CreatedUtc { get; }
}