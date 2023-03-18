using Newtonsoft.Json;

namespace Calculator.Application.Parameters;

public class DeactivateAccountParameters
{
    public Guid AccountId { get; init; }

    [JsonConstructor]
    public DeactivateAccountParameters(Guid accountId)
    {
        AccountId = accountId;
    }
}