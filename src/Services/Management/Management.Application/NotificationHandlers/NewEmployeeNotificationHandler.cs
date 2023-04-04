using Management.Application.NotificationHandlers.ProcessNotifications;
using Management.Domain.Events;
using MediatR; 
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;
using Shared.Implementations.Services;

namespace Management.Application.NotificationHandlers;

public class NewEmployeeNotificationHandler : INotificationHandler<DomainNotification<EmployeeCreated>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IEventBus _eventBus;

    public NewEmployeeNotificationHandler(ICurrentUser currentUser, IEventBus eventBus)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }
    public async Task Handle(DomainNotification<EmployeeCreated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification?.DomainEvent;
        if (domainEvent == null)
        {
            throw new ArgumentException(nameof(domainEvent) + "cannot be null.");
        }

        var processEvent = new NewAccountProcess(
            domainEvent.AccountOwnerCode,
            _currentUser.OrganizationCode!,
            _currentUser.VerificationCode!);

        await _eventBus.CommitAsync(processEvent);
    }
}