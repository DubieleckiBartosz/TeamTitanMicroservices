using Calculator.Application.Parameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.AddWorkDay;

public record AddWorkDayCommand(DateTime Date, int HoursWorked, int Overtime, bool IsDayOff, Guid AccountId) : ICommand<Unit>
{
    public static AddWorkDayCommand Create(AddWorkDayParameters parameters)
    {
        return new AddWorkDayCommand(parameters.Date, parameters.HoursWorked, parameters.Overtime, parameters.IsDayOff,
            parameters.AccountId);
    }
}