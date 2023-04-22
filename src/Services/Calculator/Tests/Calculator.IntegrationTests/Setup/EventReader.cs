using Newtonsoft.Json;
using Shared.Domain.Base;
using Shared.Implementations.EventStore;
using Shared.Implementations.Snapshot;
using Shared.Implementations.Tools;

namespace Calculator.IntegrationTests.Setup;

public class EventReader<TRead> where TRead : IRead
{
    private JsonSerializerSettings _settings = new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.All};
    public TRead Reader { get; set; }
    public List<IEvent> Events { get; set; }

    public EventReader(TRead reader, List<IEvent> events)
    {
        Reader = reader;
        Events = events;
    }

    public List<StreamState> ToStreamList<TAggregate>(Guid streamId)
    {
        var response = new List<StreamState>();

        var assemblyQualifiedName = typeof(TAggregate).AssemblyQualifiedName!;

        foreach (var @event in Events)
        {
            var qualifiedName = @event.GetType().AssemblyQualifiedName!;

            var stream = new StreamState(streamId, qualifiedName,
                assemblyQualifiedName, JsonConvert.SerializeObject(@event, _settings));

            response.Add(stream);
        }
    

        return response;
    }

    public SnapshotState ToSnapshot<TSnapshot, TAggregate>(Guid streamId) where TAggregate : Aggregate where TSnapshot : ISnapshot
    {
        var streams = ToStreamList<TAggregate>(streamId);
        var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true)!;

        var version = 0;
        foreach (var @event in streams)
        {
            var data = @event.StreamData.DeserializeEvent();

            if (data == null)
            {
                continue;
            }

            aggregate.Apply(data);
            aggregate.SetNewValue(nameof(aggregate.Version), ++version);
        }

        var snapshot = aggregate.CreateSnapshot();

        var assemblyQualifiedName = typeof(TSnapshot).AssemblyQualifiedName!;
 
        var snapshotState = new SnapshotState(aggregate.Id, aggregate.Version,
            assemblyQualifiedName, JsonConvert.SerializeObject(snapshot, _settings));
        return snapshotState;
    }
}