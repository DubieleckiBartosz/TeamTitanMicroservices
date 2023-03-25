using Newtonsoft.Json;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Snapshot;

namespace Shared.Implementations.EventStore.Repositories;

public class Repository<TAggregate> : IRepository<TAggregate> where TAggregate : Aggregate
{
    private readonly IEventStore _eventStore;
    private readonly IEventBus _eventBus;
    private readonly ISnapshotStore _snapshotStore;

    public Repository(IEventStore eventStore, IEventBus eventBus, ISnapshotStore snapshotStore)
    {
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _snapshotStore = snapshotStore ?? throw new ArgumentNullException(nameof(snapshotStore));
    }


    public async Task<TAggregate?> GetAggregateFromSnapshotAsync<TSnapshot>(Guid id) where TSnapshot : ISnapshot
    {
        var snapshotState = await _snapshotStore.GetLastSnapshotAsync(id);
        var result = await _eventStore.AggregateFromSnapshotAsync<TAggregate, TSnapshot>(id, snapshotState);

        return result;
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

    public async Task UpdateWithSnapshotAsync<TSnapshot>(TAggregate aggregate)
    {
        //[TODO] move to the snapshot repo and optimize
        await _eventStore.StoreAsync(aggregate, null);

        var assemblyQualifiedName = typeof(TSnapshot).AssemblyQualifiedName!;
        var snapshot = aggregate.CreateSnapshot();

        var stream = new SnapshotState(aggregate.Id, aggregate.Version,
            assemblyQualifiedName, JsonConvert.SerializeObject(snapshot,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }));
         
        await _snapshotStore.AddAsync(stream);
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