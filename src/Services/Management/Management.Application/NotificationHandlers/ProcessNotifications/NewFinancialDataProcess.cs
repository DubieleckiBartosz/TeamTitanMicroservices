using Shared.Domain.Abstractions;

namespace Management.Application.NotificationHandlers.ProcessNotifications;

public record NewFinancialDataProcess(Guid AccountId, decimal? Salary, decimal? HourlyRate, decimal? OvertimeRate) : IEvent;