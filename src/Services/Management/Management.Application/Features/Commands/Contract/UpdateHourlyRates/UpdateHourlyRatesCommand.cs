using Management.Application.Parameters.ContractParameters;
using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdateHourlyRates;

public record UpdateHourlyRatesCommand
    (int ContractId, decimal? HourlyRate, decimal? OvertimeRate) : ICommand<Unit>
{
    public static UpdateHourlyRatesCommand Create(UpdateHourlyRatesParameters parameters)
    {
        return new UpdateHourlyRatesCommand(parameters.ContractId, parameters.HourlyRate,
            parameters.OvertimeRate);
    }
}