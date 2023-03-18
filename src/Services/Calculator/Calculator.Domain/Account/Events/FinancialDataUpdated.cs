using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record FinancialDataUpdated(decimal? OvertimeRate, decimal? HourlyRate, Guid AccountId) : IEvent
{
    public static FinancialDataUpdated Create(decimal? overtimeRate, decimal? hourlyRate, Guid accountId)
    {
        return new FinancialDataUpdated(overtimeRate, hourlyRate, accountId);
    }
}