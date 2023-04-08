using Shared.Domain.Abstractions;

namespace Management.Domain.Events;

public record DayOffRequestConsidered(string EmployeeContact, bool PositiveDecision) : IDomainNotification;