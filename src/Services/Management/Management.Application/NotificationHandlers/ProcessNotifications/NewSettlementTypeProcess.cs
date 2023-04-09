using Shared.Domain.Abstractions;

namespace Management.Application.NotificationHandlers.ProcessNotifications;

public record NewSettlementTypeProcess(Guid AccountId, int SettlementType) : IEvent;