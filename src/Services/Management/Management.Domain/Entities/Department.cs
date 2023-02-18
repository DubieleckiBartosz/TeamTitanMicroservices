using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class Department : Entity
{
    public DepartmentName DepartmentName { get; }
    public string DepartmentUniqueCode { get; }
    public List<Employee> Employees { get; set; }

    private Department(DepartmentName departmentName)
    {
        DepartmentUniqueCode = Guid.NewGuid().ToString();
        DepartmentName = departmentName;
        Employees = new List<Employee>();
    }

    public static Department CreateDepartment(DepartmentName departmentName)
    {
        return new Department(departmentName);
    }
}