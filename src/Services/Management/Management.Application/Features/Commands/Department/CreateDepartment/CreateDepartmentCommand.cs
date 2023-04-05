using Management.Application.Parameters.DepartmentParameters;
using MediatR;

namespace Management.Application.Features.Commands.Department.CreateDepartment;

public record CreateDepartmentCommand(string DepartmentName) : ICommand<Unit>
{
    public static CreateDepartmentCommand Create(CreateDepartmentParameters parameters)
    {
        return new CreateDepartmentCommand(parameters.DepartmentName);
    }
}