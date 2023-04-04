using Newtonsoft.Json;

namespace Management.Application.Parameters.EmployeeParameters;

public class UpdateContactDataParameters
{
    public int EmployeeId { get; init; }
    public string? PhoneNumber { get; init; } 
    public string? Email { get; init; }

    //For tests
    public UpdateContactDataParameters()
    {
    }

    [JsonConstructor]
    public UpdateContactDataParameters(string? phoneNumber, string? email, int employeeId)
    {
        PhoneNumber = phoneNumber;
        Email = email;
        EmployeeId = employeeId;
    }
}