using Calculator.Application.Constants;
using Shared.Domain.Abstractions;
using Shared.Implementations.EventStore;

namespace Calculator.Application.Processes.ProcessingAccountDataCompletion;

[EventQueue(routingKey: Keys.DataCompletionQueueRoutingKey)]
public record AccountDataCompleted(int CountingType, int Status, int WorkDayHours,
    int SettlementDayMonth, Guid AccountId, DateTime? ExpirationDate) : IEvent;