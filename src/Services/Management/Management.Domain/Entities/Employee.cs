using Shared.Domain.Base;

namespace Management.Domain.Entities;

public class Employee : Entity
{
    public Guid? AccountId { get; private set; }
    public string Name { get; }
    public string Surname { get; }
    public DateTime Birthday { get; }
    public string PersonalIdentificationNumber { get; }
    public ContactData ContactData { get; }
    public List<EmployeeContract> Contracts { get; }
    public List<DayOffRequest> DayOffRequests { get; }

    private Employee(string name, string surname, DateTime birthday, string personalIdentificationNumber,
        ContactData contactData, List<EmployeeContract> contracts, List<DayOffRequest> dayOffRequests)
    {
        Name = name;
        Surname = surname;
        Birthday = birthday;
        PersonalIdentificationNumber = personalIdentificationNumber;
        ContactData = contactData;
        Contracts = contracts;
        DayOffRequests = dayOffRequests;
    }
    public void AssignAccount(Guid accountId)
    {
        AccountId = accountId;
    }
}