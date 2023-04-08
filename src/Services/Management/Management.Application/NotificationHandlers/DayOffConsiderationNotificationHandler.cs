using Management.Domain.Events;
using MediatR;
using Shared.Implementations.Decorators;

namespace Management.Application.NotificationHandlers;

public class DayOffConsiderationNotificationHandler : INotificationHandler<DomainNotification<DayOffRequestConsidered>>
{
    public DayOffConsiderationNotificationHandler()
    {
    }

    public Task Handle(DomainNotification<DayOffRequestConsidered> notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}