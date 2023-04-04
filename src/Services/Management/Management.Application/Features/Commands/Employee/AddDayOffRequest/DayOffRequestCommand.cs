using Management.Application.Parameters.EmployeeParameters;
using Management.Application.ValueTypes;
using MediatR;

namespace Management.Application.Features.Commands.Employee.AddDayOffRequest;

public record DayOffRequestCommand(DateTime From, DateTime To, DayOffRequestReasonType ReasonType,
    string? Description) : ICommand<Unit>
{
    public static DayOffRequestCommand Create(DayOffRequestParameters parameters)
    {
        var from = parameters.From;
        var to = parameters.To;
        var reasonType = parameters.ReasonType;
        var description = parameters.Description;
        return new DayOffRequestCommand(from, to, reasonType, description);
    }
}