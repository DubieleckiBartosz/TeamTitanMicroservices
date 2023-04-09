using Shared.Domain.Abstractions;

namespace Management.Domain.Events;

public record SalaryChanged(Guid AccountId, decimal NewSalary) : IDomainNotification;