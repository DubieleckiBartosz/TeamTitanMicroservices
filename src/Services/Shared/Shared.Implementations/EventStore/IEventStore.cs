using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Implementations.Projection;
using Shared.Implementations.Snapshot;

namespace Shared.Implementations.EventStore;

public interface IEventStore
{
    Task<TAggregate> AggregateStreamAsync<TAggregate>(Guid streamId, long? atStreamVersion = null,
        DateTime? atTimestamp = null) where TAggregate : Aggregate;

    Task<TAggregate?> AggregateFromSnapshotAsync<TAggregate>(Guid streamId, SnapshotState? snapshotState)
        where TAggregate : Aggregate;
    Task<IReadOnlyList<StreamState>?> GetEventsAsync(Guid streamId, long? atStreamVersion = null,
        DateTime? atTimestamp = null);

    Task AppendEventAsync<TAggregate>(Guid streamId, IEvent @event, long? expectedVersion = null,
        Func<StreamState, string?, Task>? action = null) where TAggregate : Aggregate;

    Task StoreAsync<TAggregate>(TAggregate aggregate, Func<StreamState, string?, Task>? action = null)
        where TAggregate : Aggregate;
    void RegisterProjection(IProjection projection);
}