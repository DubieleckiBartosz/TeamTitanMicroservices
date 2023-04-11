using Shared.Domain.Abstractions;
using Shared.Implementations.Outbox;
using Shared.Implementations.Tools;
using System.Text;

namespace Shared.Implementations.EventStore;

public class EventBus : IEventBus
{
    private readonly IDomainDecorator _domainDecorator;
    private readonly IOutboxListener _outboxListener;

    public EventBus(IDomainDecorator domainDecorator, IOutboxListener outboxListener)
    {
        _domainDecorator = domainDecorator ?? throw new ArgumentNullException(nameof(domainDecorator));
        _outboxListener = outboxListener ?? throw new ArgumentNullException(nameof(outboxListener));
    }
    public async Task PublishLocalAsync(params IEvent[] events)
    {
        foreach (var @event in events)
        {
            await _domainDecorator.Publish(@event);
        }
    }

    public async Task CommitAsync(params IEvent[] events)
    {
        foreach (var @event in events)
        {
            await SendToMessageBroker(@event);
        }
    }

    public async Task CommitStreamAsync(StreamState stream, string? key = null)
    {
        var queueKey = key ?? stream.EventType.CreateAlternativeKey();
        var message = new OutboxMessage(stream.EventType, stream.StreamData, queueKey);

        await _outboxListener.Commit(message);
    }

    private async Task SendToMessageBroker(IEvent @event)
    {
        await _outboxListener.Commit(@event);
    } 
}
