using Newtonsoft.Json;

namespace Calculator.Application.Parameters.Bonus;

public class AddBonusProgramParameters
{
    public decimal BonusAmount { get; init; }
    public string CompanyCode { get; init; }
    public DateTime? Expires { get; init; }
    public string Reason { get; init; }

    [JsonConstructor]
    public AddBonusProgramParameters(decimal bonusAmount, string companyCode, DateTime? expires, string reason)
    {
        BonusAmount = bonusAmount;
        CompanyCode = companyCode;
        Expires = expires;
        Reason = reason;
    }
}