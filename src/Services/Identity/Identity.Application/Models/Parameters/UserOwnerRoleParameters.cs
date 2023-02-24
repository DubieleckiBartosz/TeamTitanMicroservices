using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class UserOwnerRoleParameters
{
    public string Email { get; set; }
    public string CompanyId { get; set; }

    [JsonConstructor]
    public UserOwnerRoleParameters(string email, string companyId)
    {
        Email = email;
        CompanyId = companyId;
    }
}