using Shared.Implementations.Types;

namespace Shared.Implementations.Snapshot;

public class SnapshotState : IIdentifier
{
    public SnapshotState(Guid aggregateId, int currentVersion, string snapshotType, string snapshotData)
    {
        Id = Guid.NewGuid();
        AggregateId = aggregateId;
        CurrentVersion = currentVersion;
        SnapshotType = snapshotType;
        SnapshotData = snapshotData;
        Created = DateTime.UtcNow;
    }
    public SnapshotState()
    {
    }
     
    public Guid Id { get; private set; }
    public Guid AggregateId { get; set; }
    public int CurrentVersion { get; set; }
    public string SnapshotType { get; set; }
    public string SnapshotData { get; set; }
    public DateTime Created { get; private set; }
}