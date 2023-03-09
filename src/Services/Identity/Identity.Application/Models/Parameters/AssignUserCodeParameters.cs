using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class AssignUserCodeParameters
{
    public string Code { get; init; } 

    [JsonConstructor]
    public AssignUserCodeParameters(string code)
    {
        Code = code;
    }
}