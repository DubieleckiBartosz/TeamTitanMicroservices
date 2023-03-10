using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ChangeDayHours;

public record ChangeDayHoursCommand(int NewWorkDayHours, Guid AccountId) : ICommand<Unit>
{
}