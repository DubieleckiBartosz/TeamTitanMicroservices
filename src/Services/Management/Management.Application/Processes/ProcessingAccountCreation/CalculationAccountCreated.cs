using Management.Application.Constants;
using Shared.Domain.Abstractions;
using Shared.Implementations.EventStore;

namespace Management.Application.Processes.ProcessingAccountCreation;

[EventQueue(routingKey: Keys.NewCalculationAccountCreatedQueueRoutingKey)]
public record CalculationAccountCreated(string AccountOwnerCode, Guid AccountId) : IEvent;