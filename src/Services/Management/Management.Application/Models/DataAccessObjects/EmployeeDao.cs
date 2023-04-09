using Management.Domain.Entities;
using Management.Domain.ValueObjects;

namespace Management.Application.Models.DataAccessObjects;

public class EmployeeDao
{
    public int Id { get; init; }
    public int Version { get; init; }
    public Guid? AccountId { get; init; }
    public int DepartmentId { get; init; }
    public string Leader { get; init; } = default!;
    public string EmployeeCode { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Surname { get; init; } = default!;
    public DateTime Birthday { get; init; }
    public string? PersonIdentifier { get; init; }
    public CommunicationDao? Communication { get; set; }   
    public List<ContractDao> Contracts { get; set; } = new();
    public List<DayOffRequestDao> DayOffRequests { get; set; } = new();

    public Employee Map()
    {
        if (Communication == null)
        {
            return Employee.Load(Id, Version, Leader, EmployeeCode, Name, Surname, PersonIdentifier, AccountId);
        }

        var address = Address.Create(Communication.City, Communication.Street, Communication.NumberStreet,
            Communication.PostalCode);
        var contact = Contact.Create(Communication.PhoneNumber, Communication.Email);
        var communicationData = CommunicationData.Load(address, contact, Communication.Version);
        var contracts = Contracts.Select(_ => _.Map()).ToList();
        var dayOffRequests = DayOffRequests.Select(_ => _.Map()).ToList();

        return Employee.Load(Id, Version, Leader, DepartmentId, AccountId, EmployeeCode, Name, Surname, Birthday,
            PersonIdentifier,
            communicationData, contracts, dayOffRequests);
    }
}