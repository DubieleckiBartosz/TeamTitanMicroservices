using Shared.Domain.Abstractions;

namespace Shared.Implementations.Outbox;

public interface IOutboxListener
{
    Task Commit(OutboxMessage message);
    Task Commit<TEvent>(TEvent @event) where TEvent : IEvent;
}