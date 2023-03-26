using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class Department : Entity
{
    private readonly HashSet<Employee> _employees = new();
    public string CompanyCode { get; }
    public DepartmentName DepartmentName { get; }
    public List<Employee> Employees => _employees.ToList();

    private Department(DepartmentName departmentName, string companyCode)
    {
        CompanyCode = companyCode;
        DepartmentName = departmentName;
    }

    public static Department CreateDepartment(DepartmentName departmentName, string companyCode)
    {
        return new(departmentName, companyCode);
    }

    //var code = EmployeeCodeGenerator.Generate(CompanyCode);
    public void AddNewEmployee(string code, string name, string surname, DateTime birthday,
        string? personIdentifier, string city, string street, string numberStreet, string postalCode,
        string phoneNumber, string email)
    {
        var address = Address.Create(city, street, numberStreet, postalCode);
        var contact = Contact.Create(phoneNumber, email);

        var communicationData = CommunicationData.Create(address, contact);
        var newEmployee = Employee.Create(code, name, surname, birthday, personIdentifier, communicationData);

        //Validation
        _employees.Add(newEmployee);
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