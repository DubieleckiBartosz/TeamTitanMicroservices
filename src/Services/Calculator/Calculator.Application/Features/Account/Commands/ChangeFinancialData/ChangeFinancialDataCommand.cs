using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ChangeFinancialData;

public record ChangeFinancialDataCommand(decimal? PayoutAmount, decimal? OvertimeRate, decimal? HourlyRate, Guid AccountId) : ICommand<Unit>
{
    public static ChangeFinancialDataCommand Create(decimal? payoutAmount, decimal? overtimeRate, decimal? hourlyRate, Guid accountId)
    {
        return new ChangeFinancialDataCommand(payoutAmount, overtimeRate, hourlyRate, accountId);
    }
}