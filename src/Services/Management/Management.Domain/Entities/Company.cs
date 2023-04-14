using Management.Domain.Events;
using Management.Domain.Statuses;
using Management.Domain.ValueObjects;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;

namespace Management.Domain.Entities;

public class Company : Entity, IAggregateRoot
{
    private readonly HashSet<Department> _departments = new();
    public int OwnerId { get; }
    public CommunicationData? CommunicationData { get; private set; }
    public string CompanyCode { get; }
    public string OwnerCode { get; }
    public bool IsConfirmed { get; private set; }
    public CompanyName? CompanyName { get; private set; }
    public OpeningHours? OpeningHours { get; private set; }
    public CompanyStatus CompanyStatus { get; private set; }
    public List<Department> Departments => _departments.ToList();


    /// <summary>
    /// Creating new company
    /// </summary> 
    /// <param name="ownerId"></param> 
    /// <param name="companyCode"></param>
    /// <param name="ownerCode"></param>
    private Company(int ownerId, string companyCode, string ownerCode)
    {
        IsConfirmed = false;
        OwnerCode = ownerCode;
        OwnerId = ownerId;
        CompanyCode = companyCode;
        CompanyStatus = CompanyStatus.Init;

        Events.Add(new CompanyDeclared(CompanyCode, OwnerCode));
    }

    /// <summary>
    /// Load necessary data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="version"></param>
    /// <param name="ownerId"></param>
    /// <param name="companyCode"></param>
    /// <param name="ownerCode"></param>
    /// <param name="companyStatus"></param>
    /// <param name="isConfirmed"></param>
    private Company(int id, int version, int ownerId, string companyCode, string ownerCode, CompanyStatus companyStatus,
        bool isConfirmed) : this(ownerId, companyCode, ownerCode)
    {
        Id = id;
        Version = version;
        CompanyStatus = companyStatus;
        IsConfirmed = isConfirmed;
    }


    /// <summary>
    /// Load all data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="version"></param>
    /// <param name="ownerId"></param>
    /// <param name="ownerCode"></param>
    /// <param name="communicationData"></param>
    /// <param name="companyCode"></param>
    /// <param name="companyName"></param>
    /// <param name="openingHours"></param>
    /// <param name="companyStatus"></param>
    /// <param name="departments"></param>
    /// <param name="isConfirmed"></param>
    private Company(int id, int version, int ownerId, string ownerCode, CommunicationData communicationData, string companyCode,
        CompanyName? companyName, OpeningHours? openingHours, CompanyStatus companyStatus, List<Department> departments, bool isConfirmed)  
        : this(ownerId, companyCode, ownerCode)
    {
        Id = id;
        Version = version;
        CommunicationData = communicationData;
        CompanyName = companyName;
        OpeningHours = openingHours;
        CompanyStatus = companyStatus;
        IsConfirmed = isConfirmed;

        departments.ForEach(_ => _departments.Add(_));
    } 
    public static Company Init(int ownerId, string uniqueCode, string ownerCode)
    {
        return new Company(ownerId, uniqueCode, ownerCode);
    }

    public static Company Load(int id, int version, int ownerId, string ownerCode, CommunicationData communicationData,
        string companyCode,
        CompanyName? companyName, OpeningHours? openingHours, CompanyStatus companyStatus, List<Department> departments, bool isConfirmed)
    {
        return new Company(id, version, ownerId, ownerCode, communicationData, companyCode,
            companyName, openingHours, companyStatus, departments, isConfirmed);
    }

    public static Company Load(int id, int version, int ownerId, string companyCode, string ownerCode, CompanyStatus companyStatus, bool isConfirmed)
    {
        return new Company(id, version, ownerId, ownerCode, companyCode, companyStatus, isConfirmed);
    }


    public void UpdateData(CompanyName companyName, OpeningHours? openingHours, CommunicationData communicationData)
    {
        OpeningHours = openingHours;
        CommunicationData = communicationData; 
        CompanyName = companyName;

        IsConfirmed = true;
        CompanyStatus = CompanyStatus.Active;
        IncrementVersion();
    }

    public void AddNewDepartment(string name)
    {
        var departmentName = DepartmentName.Create(name);
        var currentDepartment = this.FindDepartmentByName(departmentName);
        if (currentDepartment != null)
        {
            throw new BusinessException("Duplicate name", "The department name should be unique");
        }

        var newDepartment = Department.CreateDepartment(departmentName); 
        this._departments.Add(newDepartment);
        IncrementVersion();
    }

    public void UpdateCommunicationData(
        string? phoneNumber, string? email, string? city, string? street,
        string? numberStreet, string? postalCode)
    {
        phoneNumber ??= this.CommunicationData!.Contact.PhoneNumber;
        email ??= this.CommunicationData!.Contact.PhoneNumber;
        city ??= this.CommunicationData!.Contact.PhoneNumber;
        street ??= this.CommunicationData!.Contact.PhoneNumber;
        numberStreet ??= this.CommunicationData!.Contact.PhoneNumber;
        postalCode ??= this.CommunicationData!.Contact.PhoneNumber;

        var newAddress = Address.Create(city, street, numberStreet, postalCode);
        CommunicationData!.UpdateAddress(newAddress);

        var newContact = Contact.Create(phoneNumber, email);
        CommunicationData.UpdateContact(newContact);
        IncrementVersion();
    }

    private Department? FindDepartmentByName(DepartmentName departmentName)
    {
        return _departments.FirstOrDefault(_ => _.DepartmentName.Equals(departmentName));
    } 
}