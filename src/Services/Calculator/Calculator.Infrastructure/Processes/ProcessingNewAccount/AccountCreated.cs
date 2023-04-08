using Calculator.Application.Constants;
using Shared.Domain.Abstractions;
using Shared.Implementations.EventStore;

namespace Calculator.Infrastructure.Processes.ProcessingNewAccount;

[EventQueue(routingKey: Keys.NewAccountQueueRoutingKey)] 
public record AccountCreated(string CompanyCode, string AccountOwnerCode, string Creator) : IEvent;