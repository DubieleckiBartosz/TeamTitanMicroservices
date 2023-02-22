using Calculator.Domain.Types;
using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record CountingTypeChanged(CountingType NewCountingType, Guid AccountId) : IEvent
{
    public static CountingTypeChanged Create(CountingType newCountingType, Guid accountId)
    {
        return new CountingTypeChanged(newCountingType, accountId);
    }
}