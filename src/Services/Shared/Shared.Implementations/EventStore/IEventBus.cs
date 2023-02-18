using Shared.Domain.Abstractions;

namespace Shared.Implementations.EventStore;

public interface IEventBus
{
    Task PublishLocalAsync(params IEvent[] events);
    Task CommitStreamAsync(StreamState stream, string? key = null);
    Task CommitAsync(params IEvent[] events); 
}