using Identity.Application.Models.Parameters;

namespace Identity.Application.Models.DataTransferObjects;

public class ResetPasswordDto
{
    public string Token { get; }
    public string Password { get; }
    public string ConfirmPassword { get; }

    public ResetPasswordDto(ResetPasswordParameters parameters)
    {
        Token = parameters.Token;
        Password = parameters.Password;
        ConfirmPassword = parameters.ConfirmPassword;
    }
}