using Shared.Domain.Abstractions;

namespace Management.Application.NotificationHandlers.ProcessNotifications;

public record NewHourlyRatesProcess(Guid AccountId, decimal? HourlyRate, decimal? OvertimeRate) : IEvent;