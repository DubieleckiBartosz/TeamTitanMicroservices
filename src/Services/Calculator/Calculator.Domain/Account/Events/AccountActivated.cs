using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record AccountActivated(string ActivatedBy, Guid AccountId) : IEvent
{
    public static AccountActivated Create(string activatedBy, Guid accountId)
    {
        return new AccountActivated(activatedBy, accountId);
    }
}