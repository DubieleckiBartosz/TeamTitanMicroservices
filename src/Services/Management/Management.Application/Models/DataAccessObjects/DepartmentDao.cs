using Management.Domain.Entities;

namespace Management.Application.Models.DataAccessObjects;

public class DepartmentDao
{
    public int Id { get; init; }
    public int Version { get; init; }
    public string DepartmentName { get; init; } = default!;
    public List<EmployeeDao> Employees { get; set; } = new();

    public Department Map()
    {
        var name = Domain.ValueObjects.DepartmentName.Create(DepartmentName);
        var employees = Employees.Select(_ => _.Map()).ToList();
        return Department.LoadDepartment(Id, Version, name, employees);
    }
}