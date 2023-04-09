using Shared.Domain.Abstractions;

namespace Management.Application.NotificationHandlers.ProcessNotifications;

public record NewSalaryProcess(Guid AccountId, decimal NewSalary) : IEvent;