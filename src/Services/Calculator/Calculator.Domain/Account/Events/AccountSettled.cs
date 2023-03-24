using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record AccountSettled(Guid AccountId, decimal Balance, DateTime From, DateTime To) : IEvent
{
    public static AccountSettled Create(Guid accountId, decimal balance, DateTime from, DateTime to)
    {
        return new AccountSettled(accountId, balance, from, to);
    }
}