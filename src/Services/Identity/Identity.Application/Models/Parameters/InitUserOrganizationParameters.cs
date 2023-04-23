using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class InitUserOrganizationParameters
{
    public string? Mail { get; init; }
    public string Role { get; init; }  
    public string UserCode { get; init; }

    public InitUserOrganizationParameters()
    {
    }

    [JsonConstructor]
    public InitUserOrganizationParameters(string userCode, string mail, string role)
    {
        UserCode = userCode; 
        Mail = mail;
        Role = role;
    }
}