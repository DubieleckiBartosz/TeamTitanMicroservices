using Management.Application.NotificationHandlers.ProcessNotifications;
using Management.Domain.Events;
using MediatR;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Management.Application.NotificationHandlers;
public class NewDayHoursNotificationHandler : INotificationHandler<DomainNotification<DayHoursChanged>>
{
    private readonly IEventBus _eventBus;

    public NewDayHoursNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }
    public async Task Handle(DomainNotification<DayHoursChanged> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification?.DomainEvent;
        if (domainEvent == null)
        {
            throw new ArgumentException(nameof(domainEvent) + "cannot be null.");
        }

        var processEvent = new NewDayHoursProcess(domainEvent.AccountId, domainEvent.NewWorkDayHours);

        await _eventBus.CommitAsync(processEvent);
    }
}