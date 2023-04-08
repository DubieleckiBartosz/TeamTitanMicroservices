using Calculator.Application.Features.Account.Commands.InitiationAccount;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Calculator.Infrastructure.Processes.ProcessingNewAccount;

public class ProcessNewAccount : IEventHandler<AccountCreated>
{
    private readonly ICommandBus _commandBus;

    public ProcessNewAccount(ICommandBus commandBus)
    {
        _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
    }

    public async Task Handle(DomainNotification<AccountCreated> notification, CancellationToken cancellationToken)
    {
        if (notification == null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        var data = notification.DomainEvent;
        var command = InitiationAccountCommand.Create(data.CompanyCode, data.AccountOwnerCode, data.Creator);

        await _commandBus.Send(command, cancellationToken);
    }
}