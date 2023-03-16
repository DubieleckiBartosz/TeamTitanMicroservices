using Newtonsoft.Json;

namespace Calculator.Application.Parameters;

public class AddBonusToAccountParameters
{ 
    public Guid AccountId { get; init; }
    public decimal Amount { get; init; }

    [JsonConstructor]
    public AddBonusToAccountParameters(Guid accountId, decimal amount)
    {
        AccountId = accountId;
        Amount = amount;
    }
}