using Management.Application.Parameters.ContractParameters;
using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdateDayHours;

public record UpdateDayHoursCommand(int ContractId, int NumberHoursPerDay) : ICommand<Unit>
{
    public static UpdateDayHoursCommand Create(UpdateDayHoursParameters parameters)
    {
        return new UpdateDayHoursCommand(parameters.ContractId, parameters.NumberHoursPerDay);
    }
}