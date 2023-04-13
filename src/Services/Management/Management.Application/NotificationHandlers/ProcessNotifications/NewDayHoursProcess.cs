using Shared.Domain.Abstractions;

namespace Management.Application.NotificationHandlers.ProcessNotifications;

public record NewDayHoursProcess(Guid AccountId, int NewWorkDayHours) : IEvent; 