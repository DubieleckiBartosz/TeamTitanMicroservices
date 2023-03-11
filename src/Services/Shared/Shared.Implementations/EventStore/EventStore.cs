using Newtonsoft.Json;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Projection;
using Shared.Implementations.Snapshot;
using Shared.Implementations.Tools;

namespace Shared.Implementations.EventStore;

public class EventStore : IEventStore
{
    private readonly Dictionary<Type, List<IProjection>> _projections = new();
    private readonly IStore _store;

    public EventStore(IStore store, List<IProjection>? projections = null)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
        projections?.ForEach(RegisterProjection);
    }

    public async Task AppendEventAsync<TAggregate>(Guid streamId, IEvent @event, long? expectedVersion = null,
        Func<StreamState, string?, Task>? action = null) where TAggregate : Aggregate
    {
        var assemblyQualifiedName = typeof(TAggregate)?.AssemblyQualifiedName;
        if (assemblyQualifiedName != null)
        {
            var qualifiedName = @event?.GetType()?.AssemblyQualifiedName;
            if (qualifiedName != null)
            {
                var stream = new StreamState(streamId, qualifiedName,
                    assemblyQualifiedName, JsonConvert.SerializeObject(@event,
                        new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }));

                await _store.AddAsync(stream, expectedVersion);

                if (action != null)
                {
                    var attribute = Attribute.GetCustomAttribute(typeof(IEvent), typeof(EventQueueAttribute));
                    string? valueKey = null;
                    if (attribute != null)
                    {
                        valueKey = ((EventQueueAttribute)attribute)?.QueueName;
                    }

                    await action(stream, valueKey);
                }

                if (@event != null)
                {
                    await ApplyProjections(@event);
                }
            }
        }
        else
        {
            throw new EventException("Event cannot be null", "Event Is NULL");
        }
    }

    public async Task<IReadOnlyList<StreamState>?> GetEventsAsync(Guid streamId, long? atStreamVersion = null,
        DateTime? atTimestamp = null)
    {
        return await _store.GetEventsAsync(streamId, version: atStreamVersion, createdUtc: atTimestamp);
    }

    public async Task<TAggregate?> AggregateFromSnapshotAsync<TAggregate, TSnapshot>(Guid streamId,
        SnapshotState? snapshotState)
        where TAggregate : Aggregate
        where TSnapshot : ISnapshot
    {
        var aggregate = (TAggregate) Activator.CreateInstance(typeof(TAggregate), true)!;

        if (snapshotState != null)
        {
            var resultSnapshot = snapshotState.SnapshotData.DeserializeSnapshot<TSnapshot>();
            var response = aggregate.FromSnapshot(resultSnapshot!);
            var events = await _store.GetEventsAsync(streamId, snapshotState.CurrentVersion);
            if (response == null || (events == null || !events.Any()))
            {
                return response as TAggregate;
            }

            response = this.ReadEvents(response, events!);

            return response as TAggregate;
        }
        else
        {
            return await this.AggregateStreamAsync<TAggregate>(streamId); 
        } 
    }

    public async Task<TAggregate> AggregateStreamAsync<TAggregate>(Guid streamId, long? atStreamVersion = null,
        DateTime? atTimestamp = null) where TAggregate : Aggregate
    {
        var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true)!;

        var events = await GetEventsAsync(streamId, atStreamVersion, atTimestamp);

        if (events == null || !events.Any())
        {
            return aggregate;
        } 

        aggregate = this.ReadEvents(aggregate, events);
        return aggregate;
    }

    public async Task StoreAsync<TAggregate>(TAggregate aggregate, Func<StreamState, string?, Task>? action = null)
        where TAggregate : Aggregate
    {
        var events = aggregate.DequeueUncommittedEvents();

        var initialVersion = aggregate.Version - events.Count;
        foreach (var @event in events)
        {
            await AppendEventAsync<TAggregate>(aggregate.Id, @event, initialVersion++, action);
        }
    }

    public void RegisterProjection(IProjection projection)
    {
        foreach (var eventType in projection.Handles)
        {
            if (!_projections.ContainsKey(eventType))
            {
                _projections[eventType] = new List<IProjection>();
            }

            _projections[eventType].Add(projection);
        }
    }

    private async Task ApplyProjections(IEvent @event, CancellationToken ct = default)
    {
        if (!_projections.ContainsKey(@event.GetType()))
        {
            return;
        }

        foreach (var projection in _projections[@event.GetType()])
        {
            await projection.Handle(@event, ct);
        }
    }

    private TAggregate ReadEvents<TAggregate>(TAggregate aggregate, IReadOnlyList<StreamState> events)
        where TAggregate : Aggregate
    {
        var version = 0;
        foreach (var @event in events)
        {
            var data = @event.StreamData.DeserializeEvent();

            if (data == null) continue;

            aggregate.Apply(data);
            aggregate.SetNewValue(nameof(aggregate.Version), ++version);
        }

        return aggregate;
    }

}