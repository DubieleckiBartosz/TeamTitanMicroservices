using Newtonsoft.Json;

namespace Calculator.Application.Parameters.AccountParameters;

public class DeactivateAccountParameters
{
    public Guid AccountId { get; init; }

    //For tests
    public DeactivateAccountParameters()
    {
    }

    [JsonConstructor]
    public DeactivateAccountParameters(Guid accountId)
    {
        AccountId = accountId;
    }
}