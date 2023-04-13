using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ChangeDayHours;

public record ChangeDayHoursCommand(int NewWorkDayHours, Guid AccountId) : ICommand<Unit>
{
    public static ChangeDayHoursCommand Create(int newWorkDayHours, Guid accountId)
    {
        return new ChangeDayHoursCommand(newWorkDayHours, accountId);
    }
}