using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record OvertimeRateChanged(decimal NewOvertimeRate, Guid AccountId) : IEvent
{
    public static OvertimeRateChanged Create(decimal newOvertimeRate, Guid accountId)
    {
        return new OvertimeRateChanged(newOvertimeRate, accountId);
    }
}