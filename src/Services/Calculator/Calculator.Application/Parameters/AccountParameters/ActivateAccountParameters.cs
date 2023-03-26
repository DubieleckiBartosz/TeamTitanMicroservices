using Newtonsoft.Json;

namespace Calculator.Application.Parameters.AccountParameters;

public class ActivateAccountParameters
{
    public Guid AccountId { get; init; }

    public ActivateAccountParameters()
    {
    }

    [JsonConstructor]
    public ActivateAccountParameters(Guid accountId)
    {
        AccountId = accountId;
    }
}