using Shared.Domain.Abstractions;

namespace Management.Application.NotificationHandlers.ProcessNotifications;

public record NewAccountProcess(string AccountOwnerCode, string CompanyCode, string Creator) : IEvent;
