using Management.Application.Contracts.Services;
using Management.Domain.Events;
using MediatR;
using Shared.Implementations.Decorators;

namespace Management.Application.NotificationHandlers;

public class NewCompanyNotificationHandler : INotificationHandler<DomainNotification<CompanyDeclared>>
{
    private readonly IMessageService _messageService;

    public NewCompanyNotificationHandler(IMessageService messageService)
    {
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }
    public Task Handle(DomainNotification<CompanyDeclared> notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}