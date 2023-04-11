using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record FinancialDataUpdated(decimal? PaymentAmount, decimal? OvertimeRate, decimal? HourlyRate, Guid AccountId) : IEvent
{
    public static FinancialDataUpdated Create(decimal? paymentAmount, decimal? overtimeRate, decimal? hourlyRate, Guid accountId)
    {
        return new FinancialDataUpdated(paymentAmount, overtimeRate, hourlyRate, accountId);
    }
}