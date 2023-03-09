using Identity.Application.Models.Parameters;

namespace Identity.Application.Models.DataTransferObjects;

public class AssignUserCodeDto
{
    public string Code { get; } 

    public AssignUserCodeDto(AssignUserCodeParameters codeParameters)
    { 
        Code = codeParameters.Code; 
    }
}