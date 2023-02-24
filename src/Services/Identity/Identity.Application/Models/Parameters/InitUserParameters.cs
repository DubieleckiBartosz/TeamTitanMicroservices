using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class InitUserParameters
{
    public string Code { get; init; }
    public string RecipientEmail { get; init; }
    public string Role { get; }
    public string? Department { get; }

    [JsonConstructor]
    public InitUserParameters(string code, string recipientEmail, string role, string? department)
    {
        Code = code;
        RecipientEmail = recipientEmail;
        Role = role;
        Department = department;
    }
}