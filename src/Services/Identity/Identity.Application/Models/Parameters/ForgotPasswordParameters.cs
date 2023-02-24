using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class ForgotPasswordParameters
{
    [JsonConstructor]
    public ForgotPasswordParameters(string email)
    {
        Email = email;
    }

    public string Email { get; set; }
}