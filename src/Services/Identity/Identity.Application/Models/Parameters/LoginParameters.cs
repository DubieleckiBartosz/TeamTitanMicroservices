using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class LoginParameters
{
    public string Email { get; init; }
    public string Password { get; init; }

    public LoginParameters()
    {
    }

    [JsonConstructor]
    public LoginParameters(string email, string password)
    {
        Email = email;
        Password = password;
    }
}