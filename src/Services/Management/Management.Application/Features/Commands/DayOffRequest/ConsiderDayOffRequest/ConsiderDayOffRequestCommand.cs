using Management.Application.Parameters.DayOffRequestParameters;
using MediatR;

namespace Management.Application.Features.Commands.DayOffRequest.ConsiderDayOffRequest;

public record ConsiderDayOffRequestCommand(int DayOffRequestId, bool Positive) : ICommand<Unit>
{
    public static ConsiderDayOffRequestCommand Create(ConsiderDayOffRequestParameters parameters)
    {
        return new ConsiderDayOffRequestCommand(parameters.DayOffRequestId, parameters.Positive);
    }
}