using Newtonsoft.Json; 

namespace Identity.Application.Models.Parameters;

public class UserNewRoleParameters
{
    public string Email { get; set; }
    public string Role { get; set; }

    [JsonConstructor]
    public UserNewRoleParameters(string email, string role)
    {
        Email = email;
        Role = role;
    }
}