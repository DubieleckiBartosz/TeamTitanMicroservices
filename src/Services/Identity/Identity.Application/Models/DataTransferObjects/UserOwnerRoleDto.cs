using Identity.Application.Models.Parameters;

namespace Identity.Application.Models.DataTransferObjects;

public class UserOwnerRoleDto
{
    public string Email { get; }
    public string CompanyId { get; }
    public UserOwnerRoleDto(UserOwnerRoleParameters parameters)
    {
        Email = parameters.Email;
        CompanyId = parameters.CompanyId;
    }
}