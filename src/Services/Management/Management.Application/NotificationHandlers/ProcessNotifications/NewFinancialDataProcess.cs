using Shared.Domain.Abstractions;

namespace Management.Application.NotificationHandlers.ProcessNotifications;

public record NewFinancialDataProcess(Guid AccountId, decimal? HourlyRate, decimal? OvertimeRate) : IEvent;