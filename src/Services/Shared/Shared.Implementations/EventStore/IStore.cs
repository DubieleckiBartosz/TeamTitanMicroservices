namespace Shared.Implementations.EventStore;

public interface IStore
{
    Task AddAsync(StreamState stream, long? expectedVersion);

    Task<IReadOnlyList<StreamState>?> GetEventsAsync(Guid aggregateId, long? startVersion = null, long? version = null,
        DateTime? createdUtc = null);
}