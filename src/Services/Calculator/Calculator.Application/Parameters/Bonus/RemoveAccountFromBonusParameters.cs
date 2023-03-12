using Newtonsoft.Json;

namespace Calculator.Application.Parameters.Bonus;

public class RemoveAccountFromBonusParameters
{
    public Guid BonusProgram { get; init; }
    public string Account { get; init; }
    public string BonusCode { get; init; }

    [JsonConstructor]
    public RemoveAccountFromBonusParameters(Guid bonusProgram, string account, string bonusCode)
    {
        BonusProgram = bonusProgram;
        Account = account;
        BonusCode = bonusCode;
    }
}