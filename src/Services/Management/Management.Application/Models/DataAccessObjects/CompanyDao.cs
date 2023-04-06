using Management.Domain.Entities;
using Management.Domain.Statuses;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Application.Models.DataAccessObjects;

public class CompanyDao
{
    public int Id { get; init; }
    public int OwnerId { get; init; }
    public string City { get; init; } = default!;
    public string Street { get; init; } = default!;
    public string NumberStreet { get; init; } = default!;
    public string PostalCode { get; init; } = default!;
    public string CompanyCode { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string OwnerCode { get; init; } = default!;
    public bool IsConfirmed { get; init; }
    public string CompanyName { get; init; } = default!;
    public int From { get; init; }
    public int To { get; init; }
    public int CompanyStatus { get; init; }
    public List<DepartmentDao> Departments { get; init; } = new();

    public Company Map()
    {
        var address = Address.Create(City, Street, NumberStreet, PostalCode);
        var contact = Contact.Create(PhoneNumber, Email);
        var communicationData = CommunicationData.Create(address, contact);
        var companyName = Domain.ValueObjects.CompanyName.Create(CompanyName);
        var openingHours = OpeningHours.Create(From, To);
        var status = Enumeration.GetById<CompanyStatus>(CompanyStatus);
        var departments = Departments.Select(_ => _.Map()).ToList();

        var company = Company.Load(Id, OwnerId, OwnerCode, communicationData, CompanyCode, companyName, openingHours,
            status, departments);

        return company;
    }
}