using Newtonsoft.Json;

namespace Management.Application.Parameters.EmployeeParameters;

public class UpdateAddressDataParameters
{
    public int EmployeeId { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string NumberStreet { get; init; }
    public string PostalCode { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public UpdateAddressDataParameters()
    {
    }

    [JsonConstructor]
    public UpdateAddressDataParameters(int employeeId, string city, string street, string numberStreet,
        string postalCode)
    {
        EmployeeId = employeeId;
        City = city;
        Street = street;
        NumberStreet = numberStreet;
        PostalCode = postalCode;
    }
}