using MediatR;
using Shared.Domain.Abstractions;
using Shared.Implementations.Decorators;

namespace Shared.Implementations.EventStore;

public interface IEventHandler<TEvent> : INotificationHandler<DomainNotification<TEvent>> where TEvent : IEvent
{
}