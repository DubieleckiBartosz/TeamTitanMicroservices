﻿using Management.Application.Features.Commands.Employee.AssignAccount;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Management.Application.Processes.ProcessingAccountCreation;

public class ProcessAssignmentCalculationAccount : IEventHandler<CalculationAccountCreated>
{
    private readonly ICommandBus _commandBus;

    public ProcessAssignmentCalculationAccount(ICommandBus commandBus)
    {
        _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
    }
    public async Task Handle(DomainNotification<CalculationAccountCreated> notification, CancellationToken cancellationToken)
    {
        if (notification == null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        var data = notification.DomainEvent;
        var accountId = data.AccountId;
        var code = data.AccountOwnerCode;
        var command = AssignCalculationAccountCommand.Create(code, accountId);
        
        await _commandBus.Send(command, cancellationToken);
    }
}