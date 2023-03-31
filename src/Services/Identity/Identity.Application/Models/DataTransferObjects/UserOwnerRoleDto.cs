using Identity.Application.Models.Parameters;

namespace Identity.Application.Models.DataTransferObjects;

public class UserOwnerRoleDto
{
    public string Organization { get; init; }  
    public string OwnerCode { get; init; }  
    public string Recipient { get; init; }  
    public UserOwnerRoleDto(UserOwnerRoleParameters parameters)
    {
        Organization = parameters.Organization;
        OwnerCode = parameters.OwnerCode;
        Recipient = parameters.Recipient;
    }
}