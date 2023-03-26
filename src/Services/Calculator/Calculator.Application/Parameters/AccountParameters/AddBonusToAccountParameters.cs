using Newtonsoft.Json;

namespace Calculator.Application.Parameters.AccountParameters;

public class AddBonusToAccountParameters
{
    public Guid AccountId { get; init; }
    public decimal Amount { get; init; }

    public AddBonusToAccountParameters()
    {
    }

    [JsonConstructor]
    public AddBonusToAccountParameters(Guid accountId, decimal amount)
    {
        AccountId = accountId;
        Amount = amount;
    }
}