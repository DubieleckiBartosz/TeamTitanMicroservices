using Newtonsoft.Json;

namespace Calculator.Application.Parameters;

public class CancelBonusAccountParameters
{
    [JsonConstructor]
    public CancelBonusAccountParameters(Guid accountId, string bonusCode)
    {
        AccountId = accountId;
        BonusCode = bonusCode;
    }

    public Guid AccountId { get; init; }
    public string BonusCode { get; init; }
}