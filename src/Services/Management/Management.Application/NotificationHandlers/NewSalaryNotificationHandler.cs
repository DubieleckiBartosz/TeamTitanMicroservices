using Management.Application.NotificationHandlers.ProcessNotifications;
using Management.Domain.Events;
using MediatR;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Management.Application.NotificationHandlers;

public class NewSalaryNotificationHandler : INotificationHandler<DomainNotification<SalaryChanged>>
{
    private readonly IEventBus _eventBus;

    public NewSalaryNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }
    public async Task Handle(DomainNotification<SalaryChanged> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification?.DomainEvent;
        if (domainEvent == null)
        {
            throw new ArgumentException(nameof(domainEvent) + "cannot be null.");
        }

        var processEvent = new NewSalaryProcess(domainEvent.AccountId, domainEvent.NewSalary);

        await _eventBus.CommitAsync(processEvent);
    }
}