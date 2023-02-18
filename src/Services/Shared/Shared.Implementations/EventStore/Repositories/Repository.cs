using Shared.Domain.Base;
using Shared.Implementations.Core.Exceptions;

namespace Shared.Implementations.EventStore.Repositories;

public class Repository<TAggregate> : IRepository<TAggregate> where TAggregate : Aggregate
{
    private readonly IEventStore _eventStore;
    private readonly IEventBus _eventBus;

    public Repository(IEventStore eventStore, IEventBus eventBus)
    {
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<TAggregate> GetAsync(Guid id)
    {
        var result = await _eventStore.AggregateStreamAsync<TAggregate>(id);
        return result;
    }

    public async Task AddAsync(TAggregate aggregate)
    {
        await _eventStore.StoreAsync(aggregate, null);
    }

    public async Task AddAndPublishAsync(TAggregate aggregate)
    {
        await _eventStore.StoreAsync(aggregate, PublishEvent);
    }

    public async Task UpdateAsync(TAggregate aggregate)
    {
        await _eventStore.StoreAsync(aggregate, null);
    }

    public async Task UpdateAndPublishAsync(TAggregate aggregate)
    {
        await _eventStore.StoreAsync(aggregate, PublishEvent);
    }

    public async Task DeleteAsync(TAggregate aggregate)
    {
        await _eventStore.StoreAsync(aggregate, null);
    }

    public async Task DeleteAndPublishAsync(TAggregate aggregate)
    {
        await _eventStore.StoreAsync(aggregate, PublishEvent);
    }

    private async Task PublishEvent(StreamState stream, string? queueKey = null)
    {
        if (stream is null)
        {
            throw new EventException($"{nameof(stream)} was null.", "Stream Is NULL");
        }

        await _eventBus.CommitStreamAsync(stream, queueKey);
    }
}