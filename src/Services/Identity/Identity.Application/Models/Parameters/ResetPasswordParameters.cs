using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class ResetPasswordParameters
{
    [JsonConstructor]
    public ResetPasswordParameters(string token, string password, string confirmPassword)
    {
        Token = token;
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    public string Token { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}