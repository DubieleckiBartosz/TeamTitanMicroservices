using Shared.Domain.Abstractions;

namespace Management.Domain.Events;

public record ContractCreated(int CountingType, int WorkDayHours,
    int SettlementDayMonth, Guid AccountId, DateTime? ExpirationDate) : IDomainNotification;