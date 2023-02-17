namespace Management.Domain.Entities;

public class Department
{
    public string DepartmentName { get; }
    public string DepartmentUniqueCode { get; }
    public List<Employee> Employees { get; set; }
}