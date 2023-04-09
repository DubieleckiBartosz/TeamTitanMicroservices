using Calculator.Application.Constants;
using Shared.Domain.Abstractions;
using Shared.Implementations.EventStore;

namespace Calculator.Infrastructure.Processes.ProcessingCountingType;

[EventQueue(routingKey: Keys.NewSettlementTypeQueueRoutingKey)]
public record CountingTypeChanged(Guid AccountId, int SettlementType) : IEvent;