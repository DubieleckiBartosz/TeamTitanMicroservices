using Shared.Domain.Abstractions;

namespace Management.Domain.Events;

public record DayHoursChanged(Guid AccountId, int NewWorkDayHours) : IDomainNotification;