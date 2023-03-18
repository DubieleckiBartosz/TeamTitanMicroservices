using Newtonsoft.Json;

namespace Calculator.Application.Parameters;

public class ActivateAccountParameters
{
    public Guid AccountId { get; init; }

    [JsonConstructor]
    public ActivateAccountParameters(Guid accountId)
    {
        AccountId = accountId;
    }
}