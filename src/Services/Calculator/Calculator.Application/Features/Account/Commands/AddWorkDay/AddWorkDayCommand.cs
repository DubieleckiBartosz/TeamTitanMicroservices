using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.AddWorkDay;

public record AddWorkDayCommand(DateTime Date, int HoursWorked, int Overtime, bool IsDayOff, Guid AccountId) : ICommand<Unit>
{
}