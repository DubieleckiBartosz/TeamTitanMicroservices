using Identity.Application.Models.Parameters;

namespace Identity.Application.Models.DataTransferObjects;

public class InitUserDto
{
    public string Code { get; }
    public string RecipientEmail { get; }
    public string Role { get; } 

    public InitUserDto(InitUserParameters parameters)
    {
        RecipientEmail = parameters.RecipientEmail;
        Code = parameters.Code;
        Role = parameters.Role; 
    }
}