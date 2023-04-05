using Management.Application.Parameters.DayOffRequestParameters;
using MediatR;

namespace Management.Application.Features.Commands.DayOffRequest.CancelDayOffRequest;

public record CancelDayOffRequestCommand(int DayOffRequestId) : ICommand<Unit>
{ 
    public static CancelDayOffRequestCommand Create(CancelDayOffRequestParameters parameters)
    {
        return new CancelDayOffRequestCommand(parameters.DayOffRequestId);
    }
}