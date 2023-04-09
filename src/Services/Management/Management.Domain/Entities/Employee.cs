using Management.Domain.Events;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;

namespace Management.Domain.Entities;

public class Employee : Entity
{
    private readonly HashSet<EmployeeContract> _employeeContracts = new();
    private readonly HashSet<DayOffRequest> _dayOffRequests = new();
    private string FullName => Name + " " + Surname;
    public Guid? AccountId { get; private set; }
    public int DepartmentId { get; }
    public string EmployeeCode { get; }
    public string Name { get; }
    public string Surname { get; }
    public DateTime Birthday { get; } 
    public string Leader { get; private set; }
    //PESEL
    public string? PersonIdentifier { get; } 
    public CommunicationData CommunicationData { get; }
    public List<EmployeeContract> Contracts => _employeeContracts.ToList();
    public List<DayOffRequest> DayOffRequests => _dayOffRequests.ToList();
 

    /// <summary>
    /// Load base data
    /// </summary>
    /// <param name="employeeCode"></param>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="personIdentifier"></param>
    /// <param name="accountId"></param>
    /// <param name="id"></param>
    /// <param name="version"></param>
    /// <param name="leader"></param>
    private Employee(
        int id, 
        int version, 
        string leader, 
        string employeeCode,
        string name, 
        string surname,
        string? personIdentifier,
        Guid? accountId)
    {
        Id = id;
        Version = version;
        Leader = leader; 
        EmployeeCode = employeeCode;
        AccountId = accountId;
        Name = name;
        Surname = surname;
        PersonIdentifier = personIdentifier;
    }

    /// <summary>
    /// Creating user
    /// </summary>
    /// <param name="departmentId"></param>
    /// <param name="leader"></param>
    /// <param name="employeeCode"></param>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="birthday"></param>
    /// <param name="personIdentifier"></param>
    /// <param name="communicationData"></param>
    private Employee(int departmentId, string leader, string employeeCode, string name, string surname, DateTime birthday,
        string? personIdentifier, CommunicationData communicationData)
    {
        EmployeeCode = employeeCode; 
        Name = name;
        Surname = surname;
        PersonIdentifier = personIdentifier;
        Birthday = birthday; 
        CommunicationData = communicationData;
        DepartmentId = departmentId;
        Leader = leader;
    }

    /// <summary>
    /// Load all data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="leader"></param>
    /// <param name="departmentId"></param>
    /// <param name="accountId"></param> 
    /// <param name="employeeCode"></param>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="birthday"></param>
    /// <param name="personIdentifier"></param>
    /// <param name="communicationData"></param>
    /// <param name="contracts"></param>
    /// <param name="dayOffRequests"></param>
    private Employee(int id, int version, string leader, int departmentId, Guid? accountId, string employeeCode, string name,
        string surname, DateTime birthday,
        string? personIdentifier, CommunicationData communicationData, List<EmployeeContract>? contracts = null,
        List<DayOffRequest>? dayOffRequests = null) : this(departmentId, leader, employeeCode, name, surname, birthday,
        personIdentifier, communicationData)
    {
        Id = id;
        AccountId = accountId;
        Version = version;
        contracts?.ForEach(_ => _employeeContracts.Add(_));
        dayOffRequests?.ForEach(_ => _dayOffRequests.Add(_));
    }

    public static Employee Create(int departmentId, string leader, string employeeCode, string name, string surname,
        DateTime birthday,
        string? personIdentifier, CommunicationData communicationData)
    {
        return new Employee(departmentId, leader, employeeCode, name, surname, birthday, personIdentifier,
            communicationData);
    }

    public static Employee Load(int id, int version, string leader, int departmentId, Guid? accountId,
        string employeeCode,
        string name, string surname, DateTime birthday,
        string? personIdentifier, CommunicationData communicationData, List<EmployeeContract>? contracts = null,
        List<DayOffRequest>? dayOffRequests = null)
    {
        return new Employee(id, version, leader, departmentId, accountId, employeeCode, name, surname, birthday,
            personIdentifier, communicationData, contracts,
            dayOffRequests);
    }
     
    public static Employee Load(int id, int version, string leader, string employeeCode, string name, string surname,
        string? personIdentifier, Guid? accountId)
    {
        return new Employee(id, version, leader, employeeCode, name, surname, personIdentifier, accountId);
    }

    public void AssignAccount(Guid accountId)
    {
        AccountId = accountId;
        IncrementVersion();
    }

    public void AddContract(EmployeeContract contract)
    {
        if (ContractsOverlap(contract))
        {
            throw new BusinessException("Invalid time range contract", "Time range of contracts overlap");
        }

        _employeeContracts.Add(contract);

        var countingType = contract.SettlementType.Id;
        var workDayHours = contract.NumberHoursPerDay;
        var settlementDayMonth = contract.PaymentMonthDay; 
        var expirationDate = contract.TimeRange.EndContract;

        Events.Add(new ContractCreated(countingType, workDayHours, settlementDayMonth, AccountId!.Value, expirationDate));
    }
    
    public void AddDayOffRequest(DayOffRequest dayOffRequest)
    {
        if (DoesRangeDaysOffOverlap(dayOffRequest.DaysOff))
        {
            throw new BusinessException("Invalid range days", "Day off requests overlap");
        }

        _dayOffRequests.Add(dayOffRequest);
        Events.Add(new DayOffRequestCreated(Leader, FullName, CommunicationData.Contact.Email));
    }

    public void UpdateAddress(string city, string street, string numberStreet, string postalCode)
    {
        var newAddress = Address.Create(city, street, numberStreet, postalCode);
        this.CommunicationData.UpdateAddress(newAddress);
        IncrementVersion();
    }

    public void UpdateContact(string? phoneNumber, string? email)
    {
        var newContact = Contact.Create(
            phoneNumber ?? CommunicationData.Contact.PhoneNumber,
            email ?? CommunicationData.Contact.Email);

        CommunicationData.UpdateContact(newContact);
        IncrementVersion();
    }

    public void UpdateLeader(string newLeader)
    {
        Leader = newLeader;
        IncrementVersion();
    }

    public EmployeeContract? FindContract(int contractId) => Contracts.FirstOrDefault(_ => _.Id == contractId);

    private bool DoesRangeDaysOffOverlap(RangeDaysOff newRange)
    {
        return DayOffRequests.Where(_ => _.Canceled == false)
            .Any(_ => newRange.FromDate <= _.DaysOff.ToDate && newRange.ToDate >= _.DaysOff.FromDate);
    }

    private bool ContractsOverlap(EmployeeContract newRange)
    {
        return Contracts.Any(_ =>
            (newRange.TimeRange.StartContract <= _.TimeRange.EndContract) || _.TimeRange.EndContract == null);
    }
}