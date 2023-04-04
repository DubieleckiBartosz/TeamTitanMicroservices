using Management.Application.Parameters.CompanyParameters;
using MediatR;

namespace Management.Application.Features.Company.Commands.UpdateContact;

public record UpdateCompanyContactCommand(string? City, string? Street,
    string? NumberStreet, string? PostalCode, string? PhoneNumber, string? Email) : ICommand<Unit>
{
    public static UpdateCompanyContactCommand Create(UpdateCompanyContactParameters parameters)
    {
        var city = parameters.City;
        var street = parameters.Street;
        var numberStreet = parameters.NumberStreet;
        var postalCode = parameters.PostalCode;
        var phoneNumber = parameters.PhoneNumber;
        var email = parameters.Email;

        return new UpdateCompanyContactCommand(city, street, numberStreet, postalCode, phoneNumber, email);
    }
}