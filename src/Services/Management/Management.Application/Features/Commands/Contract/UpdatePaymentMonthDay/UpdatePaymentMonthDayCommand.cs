using Management.Application.Parameters.ContractParameters;
using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdatePaymentMonthDay;

public record UpdatePaymentMonthDayCommand(int EmployeeId, int NewPaymentMonthDay) : ICommand<Unit>
{
    public static UpdatePaymentMonthDayCommand Create(UpdatePaymentMonthDayParameters parameters)
    {
        return new UpdatePaymentMonthDayCommand(parameters.EmployeeId, parameters.NewPaymentMonthDay);
    }
}