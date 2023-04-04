using Shared.Domain.Abstractions;

namespace Management.Domain.Events;

public record EmployeeCreated(string AccountOwnerCode) : IDomainNotification;