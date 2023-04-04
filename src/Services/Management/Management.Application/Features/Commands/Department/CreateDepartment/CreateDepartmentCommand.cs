using MediatR;

namespace Management.Application.Features.Commands.Department.CreateDepartment;

public record CreateDepartmentCommand(string DepartmentName) : ICommand<Unit>
{
    public static CreateDepartmentCommand Create(string departmentName)
    {
        return new CreateDepartmentCommand(departmentName);
    }
}