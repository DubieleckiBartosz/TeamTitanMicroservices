using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record AccountSettled(Guid AccountId, decimal Balance) : IEvent
{
    public static AccountSettled Create(Guid accountId, decimal balance)
    {
        return new AccountSettled(accountId, balance);
    }
}