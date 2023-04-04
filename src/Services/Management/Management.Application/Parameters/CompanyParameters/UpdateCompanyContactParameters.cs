using Newtonsoft.Json;

namespace Management.Application.Parameters.CompanyParameters;

public class UpdateCompanyContactParameters
{
    public string? City { get; init; }
    public string? Street { get; init; }
    public string? NumberStreet { get; init; }
    public string? PostalCode { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }

    //For tests
    public UpdateCompanyContactParameters()
    {
    }

    [JsonConstructor]
    public UpdateCompanyContactParameters(string? city, string? street, string? numberStreet, string? postalCode,
        string? phoneNumber, string? email)
    {
        City = city;
        Street = street;
        NumberStreet = numberStreet;
        PostalCode = postalCode;
        PhoneNumber = phoneNumber;
        Email = email;
    }
}