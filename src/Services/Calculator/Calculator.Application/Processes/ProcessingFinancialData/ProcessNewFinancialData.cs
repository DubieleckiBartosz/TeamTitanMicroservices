using Calculator.Application.Features.Account.Commands.ChangeFinancialData;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Decorators;
using Shared.Implementations.EventStore;

namespace Calculator.Application.Processes.ProcessingFinancialData;

public class ProcessNewFinancialData : IEventHandler<FinancialDataChanged>
{
    private readonly ICommandBus _commandBus;

    public ProcessNewFinancialData(ICommandBus commandBus)
    {
        _commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
    }

    public async Task Handle(DomainNotification<FinancialDataChanged> notification, CancellationToken cancellationToken)
    {
        if (notification == null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        var data = notification.DomainEvent;

        var command =
            ChangeFinancialDataCommand.Create(data.Salary, data.OvertimeRate, data.HourlyRate, data.AccountId);
        await _commandBus.Send(command, cancellationToken);
    }
}