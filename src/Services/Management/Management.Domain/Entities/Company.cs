using Management.Domain.Statuses;
using Management.Domain.ValueObjects;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class Company : Entity, IAggregateRoot
{
    private readonly HashSet<Department> _departments = new();
    public int OwnerId { get; }
    public CommunicationData CommunicationData { get; private set; }
    public string CompanyCode { get; }
    public bool IsConfirmed { get; private set; }
    public CompanyName CompanyName { get; private set; }
    public OpeningHours? OpeningHours { get; private set; }
    public CompanyStatus CompanyStatus { get; private set; }
    public List<Department> Departments => _departments.ToList();


    /// <summary>
    /// Creating new company
    /// </summary> 
    /// <param name="ownerId"></param> 
    /// <param name="uniqueCode"></param>
    private Company(int ownerId, string uniqueCode)
    {
        IsConfirmed = false;
        OwnerId = ownerId;
        CompanyCode = uniqueCode;
    }

    /// <summary>
    /// Load data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ownerId"></param>
    /// <param name="communicationData"></param>
    /// <param name="companyCode"></param>
    /// <param name="companyName"></param>
    /// <param name="openingHours"></param>
    /// <param name="companyStatus"></param>
    /// <param name="departments"></param>
    private Company(int id, int ownerId, CommunicationData communicationData, string companyCode,
        CompanyName companyName, OpeningHours? openingHours, CompanyStatus companyStatus, List<Department> departments)
        : this( ownerId, companyCode)
    {
        Id = id;
        CommunicationData = communicationData;
        CompanyName = companyName;
        OpeningHours = openingHours;
        CompanyStatus = companyStatus;
        departments.ForEach(_ => _departments.Add(_));
    }

    public static Company Init(int ownerId, string uniqueCode)
    {
        return new Company(ownerId, uniqueCode);
    }

    public static Company Load(int id, int ownerId, CommunicationData communicationData, string companyCode,
        CompanyName companyName, OpeningHours? openingHours, CompanyStatus companyStatus, List<Department> departments)
    {
        return new Company(id, ownerId, communicationData, companyCode,
            companyName, openingHours, companyStatus, departments);
    }

    public void UpdateData(CompanyName companyName, OpeningHours? openingHours, CommunicationData communicationData)
    {
        OpeningHours = openingHours;
        CommunicationData = communicationData; 
        CompanyName = companyName;

        IsConfirmed = true;
        CompanyStatus = CompanyStatus.Active; 
    }

    public void AddNewDepartment(string name)
    {
        var departmentName = DepartmentName.Create(name);
        var currentDepartment = this.FindDepartmentByName(departmentName);
        if (currentDepartment == null)
        {

        }

        var newDepartment = Department.CreateDepartment(departmentName); 
        this._departments.Add(newDepartment);
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

    private Department? FindDepartmentByName(DepartmentName departmentName) => this._departments.FirstOrDefault(_ => _.DepartmentName == departmentName);
    private Department? FindDepartmentById(int departmentId) => this._departments.FirstOrDefault(_ => _.Id == departmentId);
}