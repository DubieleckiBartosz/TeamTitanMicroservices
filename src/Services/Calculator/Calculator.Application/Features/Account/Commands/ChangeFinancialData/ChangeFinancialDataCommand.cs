using Calculator.Application.Parameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ChangeFinancialData;

public record ChangeFinancialDataCommand(decimal? OvertimeRate, decimal? HourlyRate, Guid AccountId) : ICommand<Unit>
{
    public static ChangeFinancialDataCommand Create(ChangeFinancialDataParameters parameters)
    {
        return new ChangeFinancialDataCommand(parameters.OvertimeRate, parameters.HourlyRate, parameters.AccountId);
    }
}