using Management.Application.Parameters.DayOffRequestParameters;
using Management.Application.ValueTypes;
using MediatR;

namespace Management.Application.Features.Commands.DayOffRequest.AddDayOffRequest;

public record DayOffRequestCommand(DateTime From, DateTime To, DayOffRequestReasonType ReasonType,
    string? Description) : ICommand<Unit>
{
    public static DayOffRequestCommand Create(NewDayOffRequestParameters parameters)
    {
        var from = parameters.From;
        var to = parameters.To;
        var reasonType = parameters.ReasonType;
        var description = parameters.Description;
        return new DayOffRequestCommand(from, to, reasonType, description);
    }
}