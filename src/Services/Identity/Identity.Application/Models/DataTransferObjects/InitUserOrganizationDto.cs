using Identity.Application.Models.Parameters;

namespace Identity.Application.Models.DataTransferObjects;

public class InitUserOrganizationDto
{
    public string? Mail { get; }
    public string Role { get; }  
    public string UserCode { get; }

    public InitUserOrganizationDto(InitUserOrganizationParameters parameters)
    {
        Role = parameters.Role;
        Mail = parameters.Mail; 
        UserCode = parameters.UserCode;
    }
}