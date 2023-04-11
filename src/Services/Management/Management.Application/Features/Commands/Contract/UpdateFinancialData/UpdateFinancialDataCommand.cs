using Management.Application.Parameters.ContractParameters;
using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdateFinancialData;

public record UpdateFinancialDataCommand
    (decimal? Salary, int ContractId, decimal? HourlyRate, decimal? OvertimeRate) : ICommand<Unit>
{
    public static UpdateFinancialDataCommand Create(UpdateFinancialDataParameters parameters)
    {
        return new UpdateFinancialDataCommand(parameters.Salary, parameters.ContractId, parameters.HourlyRate,
            parameters.OvertimeRate);
    }
}