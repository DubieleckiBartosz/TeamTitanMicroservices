using Management.Domain.Statuses;
using Management.Domain.ValueObjects;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class Company : Entity, IAggregation
{
    public string CompanyExternalId { get; }
    public string OwnerId { get; }
    public ContactData ContactData { get; }
    public CompanyName CompanyName { get; }
    public OpeningHours? OpeningHours { get; }
    public CompanyStatus CompanyStatus { get; private set; }
    public List<Department> Departments { get; private set; }

    private Company(CompanyName companyName, string ownerId, OpeningHours? openingHours, ContactData contactData)
    {
        CompanyName = companyName;
        CompanyExternalId = Guid.NewGuid().ToString();
        OwnerId = ownerId;
        OpeningHours = openingHours;
        ContactData = contactData;
        CompanyStatus = CompanyStatus.Active;
        Departments = new List<Department>();
    }

    public static Company CreateCompany(CompanyName companyName, string ownerId, OpeningHours? openingHours, ContactData contactData)
    {
        return new Company(companyName, ownerId, openingHours, contactData);
    }

    public void AddNewDepartment(string name)
    {
        var departmentName = DepartmentName.Create(name);
        var currentDepartment = this.FindDepartmentByName(departmentName);
        if (currentDepartment == null)
        {

        }

        var newDepartment = Department.CreateDepartment(departmentName);
        Departments ??= new List<Department>();
        this.Departments.Add(newDepartment);
    }

    public void RemoveDepartment(int departmentId)
    {
        var department = this.FindDepartmentById(departmentId);
        if (department == null)
        {

        }

    }

    private Department? FindDepartmentByName(DepartmentName departmentName) => this.Departments.FirstOrDefault(_ => _.DepartmentName == departmentName);
    private Department? FindDepartmentById(int departmentId) => this.Departments.FirstOrDefault(_ => _.Id == departmentId);
}