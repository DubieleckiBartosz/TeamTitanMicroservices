using Shared.Domain.Abstractions;

namespace Management.Application.NotificationHandlers.ProcessNotifications;

public record NewContractProcess(int CountingType, int WorkDayHours,
    int SettlementDayMonth, Guid AccountId, DateTime? ExpirationDate) : IEvent;