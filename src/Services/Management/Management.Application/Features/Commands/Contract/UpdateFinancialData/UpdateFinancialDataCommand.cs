using Management.Application.Parameters.ContractParameters;
using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdateFinancialData;

public record UpdateFinancialDataCommand(int EmployeeId, decimal? HourlyRate, decimal? OvertimeRate) : ICommand<Unit>
{
    public static UpdateFinancialDataCommand Create(UpdateFinancialDataParameters parameters)
    {
        return new UpdateFinancialDataCommand(parameters.EmployeeId, parameters.HourlyRate, parameters.OvertimeRate);
    }
}