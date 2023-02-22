using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record AccountDeactivated(string DeactivatedBy, Guid AccountId) : IEvent
{
    public static AccountDeactivated Create(string deactivatedBy, Guid accountId)
    {
        return new AccountDeactivated(deactivatedBy, accountId);
    }
}