using Management.Application.Parameters.ContractParameters;
using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdateSalary;

public record UpdateSalaryCommand(int EmployeeId, decimal NewSalary) : ICommand<Unit>
{
    public static UpdateSalaryCommand Create(UpdateSalaryParameters parameters)
    {
        return new UpdateSalaryCommand(parameters.EmployeeId, parameters.NewSalary);
    }
}