using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class Employee : Entity
{
    public Guid? AccountId { get; private set; }
    public int? UserId { get; private set; } 
    public string EmployeeCode { get; }
    public string Name { get; }
    public string Surname { get; }
    public DateTime Birthday { get; }
    public string PersonalIdentificationNumber { get; }
    public ContactData ContactData { get; }
    public List<EmployeeContract> Contracts { get; }
    public List<DayOffRequest> DayOffRequests { get; }

    private Employee(string name, string surname, DateTime birthday, string personalIdentificationNumber,
        ContactData contactData, List<EmployeeContract> contracts, List<DayOffRequest> dayOffRequests, string employeeCode)
    {
        Name = name;
        Surname = surname;
        Birthday = birthday;
        PersonalIdentificationNumber = personalIdentificationNumber;
        ContactData = contactData;
        Contracts = contracts;
        DayOffRequests = dayOffRequests;
        EmployeeCode = employeeCode; 
    }
    public void AssignAccount(Guid accountId)
    {
        AccountId = accountId;
    }

    public void AssignUser(int userId)
    {
        this.UserId = userId;
    }
}