using MediatR;
using Shared.Domain.Abstractions;

namespace Shared.Implementations.Decorators;

public class DomainNotification<T> : INotification where T : IDomainNotification
{
    public T DomainEvent { get; }

    public DomainNotification(T @event)
    {
        this.DomainEvent = @event;
    }
}