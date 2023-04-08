using Management.Domain.Entities;
using Management.Domain.Statuses;
using Management.Domain.ValueObjects;
using Shared.Domain.Base;

namespace Management.Application.Models.DataAccessObjects;

public class CompanyDao
{
    public int Id { get; init; }
    public int Version { get; init; }
    public int OwnerId { get; init; }
    public string CompanyCode { get; init; } = default!;
    public CommunicationDao? Communication { get; init; }
    public string OwnerCode { get; init; } = default!;
    public bool IsConfirmed { get; init; }
    public string? CompanyName { get; init; } 
    public int From { get; init; }
    public int To { get; init; }
    public int CompanyStatus { get; init; }
    public List<DepartmentDao> Departments { get; init; } = new();

    public Company Map()
    {
        var status = Enumeration.GetById<CompanyStatus>(CompanyStatus);

        if (Communication == null)
        {
            var companyNecessaryData = Company.Load(Id, Version, OwnerId, CompanyCode, OwnerCode, status, IsConfirmed);
            return companyNecessaryData;
        }

        var city = Communication.City;
        var street = Communication.Street;
        var numberStreet = Communication.NumberStreet;
        var postalCode = Communication.PostalCode;
        var phoneNumber = Communication.PhoneNumber;
        var email = Communication.Email;
        var address = Address.Create(city, street, numberStreet, postalCode);
        var contact = Contact.Create(phoneNumber, email);
        var communicationData = CommunicationData.Load(address, contact, Communication.Version);
        var companyName = Domain.ValueObjects.CompanyName.Create(CompanyName);
        var openingHours = OpeningHours.Create(From, To);
        var departments = Departments.Select(_ => _.Map()).ToList();

        var company = Company.Load(Id, Version, OwnerId, OwnerCode, communicationData, CompanyCode, companyName,
            openingHours,
            status, departments, IsConfirmed);

        return company;
    }
}