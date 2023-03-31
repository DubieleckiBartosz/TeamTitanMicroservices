using Newtonsoft.Json;

namespace Identity.Application.Models.Parameters;

public class UserOwnerRoleParameters
{
    public string Organization { get; init; } = default!;
    public string OwnerCode { get; init; } = default!;
    public string Recipient { get; init; } = default!;

    public UserOwnerRoleParameters()
    {
    }

    [JsonConstructor]
    public UserOwnerRoleParameters(string organization, string ownerCode, string recipient)
    {
        Organization = organization;
        OwnerCode = ownerCode;
        Recipient = recipient;
    }
}