using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class AssignUserCodesParameters
{
    public string OrganizationCode { get; init; } 
    public string UserCode { get; init; } 

    [JsonConstructor]
    public AssignUserCodesParameters(string userCode, string organizationCode)
    {
        UserCode = userCode;
        OrganizationCode = organizationCode;
    }
}