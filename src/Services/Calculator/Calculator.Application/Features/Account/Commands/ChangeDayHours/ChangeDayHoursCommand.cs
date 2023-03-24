using Calculator.Application.Parameters.AccountParameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ChangeDayHours;

public record ChangeDayHoursCommand(int NewWorkDayHours, Guid AccountId) : ICommand<Unit>
{
    public static ChangeDayHoursCommand Create(ChangeDayHoursParameters parameters)
    {
        return new ChangeDayHoursCommand(parameters.NewWorkDayHours, parameters.AccountId);
    }
}