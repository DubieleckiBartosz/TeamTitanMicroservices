namespace Management.Application.Models.DataAccessObjects;

public class DepartmentDao
{
    public string DepartmentName { get; init; } = default!;
    public List<EmployeeDao> Employees { get; set; } = new();
}