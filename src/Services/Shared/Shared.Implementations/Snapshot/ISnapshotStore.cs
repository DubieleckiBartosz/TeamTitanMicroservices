namespace Shared.Implementations.Snapshot;

public interface ISnapshotStore
{
    Task AddAsync(SnapshotState state);
    Task<SnapshotState?> GetLastSnapshotAsync(Guid aggregateId);
}