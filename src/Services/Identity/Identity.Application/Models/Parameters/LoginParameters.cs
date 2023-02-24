using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class LoginParameters
{
    public string Email { get; set; }
    public string Password { get; set; }

    [JsonConstructor]
    public LoginParameters(string email, string password)
    {
        Email = email;
        Password = password;
    }
}