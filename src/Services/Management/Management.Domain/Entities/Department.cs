using Management.Domain.Events;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class Department : Entity
{
    private readonly HashSet<Employee> _employees = new();
    public DepartmentName DepartmentName { get; }
    public List<Employee> Employees => _employees.ToList();

    private Department(DepartmentName departmentName)
    {
        DepartmentName = departmentName;
    }

    public static Department CreateDepartment(DepartmentName departmentName)
    {
        return new(departmentName);
    }

    public void AddNewEmployee(string code, string name, string surname, DateTime birthday,
        string? personIdentifier, Address address, Contact contact)
    {
        var communicationData = CommunicationData.Create(address, contact);
        var newEmployee = Employee.Create(Id, code, name, surname, birthday, personIdentifier, communicationData);

        //Validation
        _employees.Add(newEmployee);
        Events.Add(new EmployeeCreated(code));
    }

    public void RemoveEmployee(int employeeId)
    {
        var employee = FindEmployee(employeeId);
        if (employee == null)
        {
        }

        Employees.Remove(employee);
    }

    private Employee? FindEmployee(int employeeId)
    {
        return Employees.FirstOrDefault(_ => _.Id == employeeId);
    }
}