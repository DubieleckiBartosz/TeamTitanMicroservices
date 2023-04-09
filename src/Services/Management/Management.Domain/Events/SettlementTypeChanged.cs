using Shared.Domain.Abstractions;

namespace Management.Domain.Events;

public record SettlementTypeChanged(Guid AccountId, int SettlementType) : IDomainNotification;