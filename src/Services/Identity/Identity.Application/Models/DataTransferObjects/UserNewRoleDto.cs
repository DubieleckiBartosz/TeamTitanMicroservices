using Identity.Application.Models.Parameters;

namespace Identity.Application.Models.DataTransferObjects;

public class UserNewRoleDto
{
    public string Email { get; }
    public string Role { get; }

    public UserNewRoleDto(UserNewRoleParameters parameters)
    {
        Email = parameters.Email;
        Role = parameters.Role;
    }
}