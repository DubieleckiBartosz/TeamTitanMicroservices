using Identity.Application.Models.Parameters;

namespace Identity.Application.Models.DataTransferObjects;

public class AssignUserCodesDto
{
    public string OrganizationCode { get; }

    public string UserCode { get; } 

    public AssignUserCodesDto(AssignUserCodesParameters codesParameters)
    { 
        UserCode = codesParameters.UserCode; 
        OrganizationCode = codesParameters.OrganizationCode;
    }
}