using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record HourlyRateChanged(decimal NewHourlyRate, Guid AccountId) : IEvent
{
    public static HourlyRateChanged Create(decimal newHourlyRate, Guid accountId)
    {
        return new HourlyRateChanged(newHourlyRate, accountId);
    }
}