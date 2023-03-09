namespace Management.Application.Features.Company.Commands.CreateCompany;

public record CreateCompanyCommand(string CompanyName, int? From, int? To, string City, string Street,
    string NumberStreet, string PostalCode, string PhoneNumber, string Email) : ICommand<int>
{
    public static CreateCompanyCommand Create(CreateCompanyParameters parameters)
    {
        var companyName = parameters.CompanyName;
        var from = parameters.From;
        var to = parameters.To;
        var city = parameters.City;
        var street = parameters.Street;
        var numberStreet = parameters.NumberStreet;
        var postalCode = parameters.PostalCode;
        var phoneNumber = parameters.PhoneNumber;
        var email = parameters.Email;

        return new CreateCompanyCommand(companyName, from, to, city, street, numberStreet, postalCode, phoneNumber,
            email);
    }
}