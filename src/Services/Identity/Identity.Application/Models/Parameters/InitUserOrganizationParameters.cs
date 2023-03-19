using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class InitUserOrganizationParameters
{
    public string? Mail { get; init; }
    public string Role { get; init; } 
    public string OrganizationCode { get; init; }
    public string UserCode { get; init; }

    [JsonConstructor]
    public InitUserOrganizationParameters(string userCode, string organizationCode, string mail, string role)
    {
        UserCode = userCode;
        OrganizationCode = organizationCode;
        Mail = mail;
        Role = role;
    }
}