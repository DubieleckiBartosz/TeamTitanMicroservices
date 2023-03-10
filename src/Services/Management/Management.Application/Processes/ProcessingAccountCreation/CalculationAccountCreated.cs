using Management.Application.Constants;
using Shared.Domain.Abstractions;
using Shared.Implementations.EventStore;

namespace Management.Application.Processes.ProcessingAccountCreation;

[EventQueue(routingKey: Keys.NewCalculationAccountCreatedQueueRoutingKey)]
public record CalculationAccountCreated(string AccountCode,Guid AccountId) : IEvent
{
    public static CalculationAccountCreated Create(string accountCode, Guid accountId)
    {
        return new CalculationAccountCreated(accountCode, accountId);
    }
}