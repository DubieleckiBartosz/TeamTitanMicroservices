using Newtonsoft.Json;

namespace Management.Application.Parameters.EmployeeParameters;

public class CreateEmployeeParameters
{
    public int DepartmentId { get; init; }
    public string? LeaderContact { get; init; } 
    public string Name { get; init; }
    public string Surname { get; init; }
    public DateTime Birthday { get; init; }
    public string? PersonIdentifier { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string NumberStreet { get; init; }
    public string PostalCode { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public CreateEmployeeParameters()
    {
    }

    [JsonConstructor]
    public CreateEmployeeParameters(int departmentId, string? leaderContact, string name, string surname,
        DateTime birthday, string? personIdentifier, string city, string street, string numberStreet, string postalCode,
        string phoneNumber, string email)
    {
        this.DepartmentId = departmentId;
        this.LeaderContact = leaderContact; 
        this.Name = name;
        this.Surname = surname;
        this.Birthday = birthday;
        this.PersonIdentifier = personIdentifier;
        this.City = city;
        this.Street = street;
        this.NumberStreet = numberStreet;
        this.PostalCode = postalCode;
        this.PhoneNumber = phoneNumber;
        this.Email = email;
    }
}