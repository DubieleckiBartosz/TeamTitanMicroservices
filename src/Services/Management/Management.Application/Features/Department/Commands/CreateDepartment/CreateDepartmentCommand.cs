using MediatR;

namespace Management.Application.Features.Department.Commands.CreateDepartment;

public record CreateDepartmentCommand(string DepartmentName) : ICommand<Unit>
{
    public static CreateDepartmentCommand Create(string departmentName)
    {
        return new CreateDepartmentCommand(departmentName);
    }
}