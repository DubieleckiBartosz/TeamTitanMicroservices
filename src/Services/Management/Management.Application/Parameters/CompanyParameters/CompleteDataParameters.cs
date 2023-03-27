using Newtonsoft.Json;

namespace Management.Application.Parameters.CompanyParameters;

public class CompleteDataParameters
{
    public string CompanyName { get; init; }
    public int? From { get; init; }
    public int? To { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string NumberStreet { get; init; }
    public string PostalCode { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }

    //For tests
    public CompleteDataParameters()
    {
    }

    [JsonConstructor]
    public CompleteDataParameters(string companyName, int? from, int? to, string city, string street,
        string numberStreet, string postalCode, string phoneNumber, string email)
    {
        CompanyName = companyName;
        From = from;
        To = to;
        City = city;
        Street = street;
        NumberStreet = numberStreet;
        PostalCode = postalCode;
        PhoneNumber = phoneNumber;
        Email = email;
    }
}