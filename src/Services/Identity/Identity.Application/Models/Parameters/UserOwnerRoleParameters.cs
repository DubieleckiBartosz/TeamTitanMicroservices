using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class UserOwnerRoleParameters
{
    public string Email { get; set; } 

    [JsonConstructor]
    public UserOwnerRoleParameters(string email)
    {
        Email = email; 
    }
}