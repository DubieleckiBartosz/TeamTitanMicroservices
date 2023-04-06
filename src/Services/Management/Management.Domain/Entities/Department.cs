using Management.Domain.Events;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;

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

    private Department(int id, DepartmentName departmentName, List<Employee> employees) : this(departmentName)
    {
        Id = id;
        employees.ForEach(_ => _employees.Add(_));
    }


    public static Department CreateDepartment(DepartmentName departmentName)
    {
        return new(departmentName);
    }
    public static Department LoadDepartment(int id, DepartmentName departmentName, List<Employee> employees)
    {
        return new(id, departmentName, employees);
    }

    public void AddNewEmployee(string code, string name, string surname, DateTime birthday,
        string? personIdentifier, Address address, Contact contact)
    {
        var communicationData = CommunicationData.Create(address, contact);
        var newEmployee = Employee.Create(Id, code, name, surname, birthday, personIdentifier, communicationData);
         
        _employees.Add(newEmployee);
        Events.Add(new EmployeeCreated(code));
    }

    public void RemoveEmployee(int employeeId)
    {
        //[TODO]
        var employee = FindEmployee(employeeId);
        if (employee == null)
        {
            throw new BusinessException("Incorrect identifier", "The employee to be removed was not found");
        }

        Employees.Remove(employee);
    }

    private Employee? FindEmployee(int employeeId)
    {
        return Employees.FirstOrDefault(_ => _.Id == employeeId);
    }
}