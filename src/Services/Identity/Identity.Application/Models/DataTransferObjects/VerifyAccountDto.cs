namespace Identity.Application.Models.DataTransferObjects;

public class VerifyAccountDto
{
    public string Token { get; }
    public VerifyAccountDto(string token)
    {
        Token = token;
    }
}