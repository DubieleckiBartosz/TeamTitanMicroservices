using Management.Domain.Statuses;
using Management.Domain.ValueObjects;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class Company : Entity, IAggregateRoot
{ 
    public int OwnerId { get; }
    public CommunicationData CommunicationData { get; }
    public string CompanyCode { get; }
    public CompanyName CompanyName { get; }
    public OpeningHours? OpeningHours { get; }
    public CompanyStatus CompanyStatus { get; private set; }
    public List<Department> Departments { get; private set; }

    private Company(CompanyName companyName, int ownerId, OpeningHours? openingHours,
        CommunicationData communicationData, string uniqueCode)
    {
        CompanyName = companyName;
        OwnerId = ownerId;
        OpeningHours = openingHours;
        CommunicationData = communicationData;
        CompanyStatus = CompanyStatus.Active;
        Departments = new List<Department>();
        CompanyCode = uniqueCode;
    }

    public static Company CreateCompany(CompanyName companyName, int ownerId, OpeningHours? openingHours,
        CommunicationData communicationData, string uniqueCode)
    {
        return new Company(companyName, ownerId, openingHours, communicationData, uniqueCode);
    }

    public void AddNewDepartment(string name)
    {
        var departmentName = DepartmentName.Create(name);
        var currentDepartment = this.FindDepartmentByName(departmentName);
        if (currentDepartment == null)
        {

        }

        var newDepartment = Department.CreateDepartment(departmentName, CompanyCode);
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
     
    public void UpdateAddress(string city, string street, string numberStreet, string postalCode)
    {
        var newAddress = Address.Create(city, street, numberStreet, postalCode);
        this.CommunicationData.UpdateAddress(newAddress);
    }

    public void UpdateContact(string phoneNumber, string email)
    {
        var newContact = Contact.Create(phoneNumber, email);
        this.CommunicationData.UpdateContact(newContact);
    }

    private Department? FindDepartmentByName(DepartmentName departmentName) => this.Departments.FirstOrDefault(_ => _.DepartmentName == departmentName);
    private Department? FindDepartmentById(int departmentId) => this.Departments.FirstOrDefault(_ => _.Id == departmentId);
}