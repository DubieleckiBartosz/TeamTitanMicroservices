using Identity.Application.Models.Parameters;

namespace Identity.Application.Models.DataTransferObjects;

public class LoginDto
{
    public string Email { get; }
    public string Password { get; }
    public LoginDto(LoginParameters parameters)
    {
        Email = parameters.Email;
        Password = parameters.Password;
    }
}