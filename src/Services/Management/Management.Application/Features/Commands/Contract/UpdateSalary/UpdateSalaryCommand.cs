using Management.Application.Parameters.ContractParameters;
using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdateSalary;

public record UpdateSalaryCommand(int ContractId, decimal NewSalary) : ICommand<Unit>
{
    public static UpdateSalaryCommand Create(UpdateSalaryParameters parameters)
    {
        return new UpdateSalaryCommand(parameters.ContractId, parameters.NewSalary);
    }
}