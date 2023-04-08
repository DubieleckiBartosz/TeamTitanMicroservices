using Management.Domain.Entities;
using Management.Domain.ValueObjects;

namespace Management.Application.Models.DataAccessObjects;

public class EmployeeDao
{
    public int Id { get; init; }
    public Guid? AccountId { get; init; }
    public int DepartmentId { get; init; }
    public string Leader { get; init; } = default!;
    public string EmployeeCode { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Surname { get; init; } = default!;
    public DateTime Birthday { get; init; }
    public string? PersonIdentifier { get; init; }
    public string City { get; init; } = default!;
    public string Street { get; init; } = default!;
    public string NumberStreet { get; init; } = default!;
    public string PostalCode { get; init; } = default!;
    public string CompanyCode { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string Email { get; init; } = default!;
    public List<ContractDao> Contracts { get; set; } = new();
    public List<DayOffRequestDao> DayOffRequests { get; set; } = new();

    public Employee Map()
    {
        var address = Address.Create(City, Street, NumberStreet, PostalCode);
        var contact = Contact.Create(PhoneNumber, Email);
        var communicationData = CommunicationData.Create(address, contact);
        var contracts = Contracts.Select(_ => _.Map()).ToList();
        var dayOffRequests = DayOffRequests.Select(_ => _.Map()).ToList();

        return Employee.Load(Id, Leader, DepartmentId, AccountId, EmployeeCode, Name, Surname, Birthday,
            PersonIdentifier,
            communicationData, contracts, dayOffRequests);
    }
}