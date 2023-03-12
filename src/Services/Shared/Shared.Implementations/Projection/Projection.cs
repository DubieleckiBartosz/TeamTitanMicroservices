using Shared.Domain.Abstractions; 

namespace Shared.Implementations.Projection;

public class Projection : IProjection
{
    private readonly Dictionary<Type, Func<IEvent, CancellationToken, Task>> _handlers = new();


    public Type[] Handles => _handlers.Keys.ToArray();

    protected void Projects<TEvent>(Func<TEvent, CancellationToken, Task> action)
    {
        _handlers.Add(typeof(TEvent), (@event, cancellationToken) => action((TEvent) @event, cancellationToken));
    }

    public Task Handle(IEvent @event, CancellationToken ct)
    {
        return _handlers[@event.GetType()](@event, ct);
    }
}

public abstract class ReadModelAction<TEntity> : Projection where TEntity : class
{
    protected void Projects(Func<IEvent, CancellationToken, Task> handler)
    {
        Projects<IEvent>(handler);
    }
}