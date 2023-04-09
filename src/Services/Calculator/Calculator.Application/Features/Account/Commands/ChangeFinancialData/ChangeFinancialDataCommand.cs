using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ChangeFinancialData;

public record ChangeFinancialDataCommand(decimal? OvertimeRate, decimal? HourlyRate, Guid AccountId) : ICommand<Unit>
{
    public static ChangeFinancialDataCommand Create(decimal? overtimeRate, decimal? hourlyRate, Guid accountId)
    {
        return new ChangeFinancialDataCommand(overtimeRate, hourlyRate, accountId);
    }
}