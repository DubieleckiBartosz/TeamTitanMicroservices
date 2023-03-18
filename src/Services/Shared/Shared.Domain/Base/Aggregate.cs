using System.Text.Json.Serialization;
using Shared.Domain.Abstractions;

namespace Shared.Domain.Base;

public abstract class Aggregate
{
    public Guid Id { get; protected set; }
    public int Version { get; protected set; }

    [JsonIgnore] private readonly Queue<IEvent>? _uncommittedEvents = new Queue<IEvent>();

    public IReadOnlyList<IEvent> DequeueUncommittedEvents()
    {
        if (_uncommittedEvents == null)
        {
            return new List<IEvent>();
        }

        var dequeuedEvents = new List<IEvent>();
        for (var i = 0; i < _uncommittedEvents.Count; i++)
        {
            dequeuedEvents.Add(_uncommittedEvents.Dequeue());
        }

        return dequeuedEvents.AsReadOnly();
    }

    protected abstract void When(IEvent @event);

    protected void Enqueue(IEvent @event)
    {
        Version++;
        _uncommittedEvents?.Enqueue(@event);
    }

    public void Apply(IEvent @event)
    {
        When(@event);
    }

    public abstract Aggregate? FromSnapshot(ISnapshot snapshot);
    public abstract ISnapshot? CreateSnapshot();
}