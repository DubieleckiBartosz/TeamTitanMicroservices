using Newtonsoft.Json;

namespace Calculator.Application.Parameters.Bonus;

public class RemoveAccountFromBonusParameters
{
    public Guid BonusProgram { get; init; }
    public string Account { get; init; }

    [JsonConstructor]
    public RemoveAccountFromBonusParameters(Guid bonusProgram, string account)
    {
        BonusProgram = bonusProgram;
        Account = account;
    }
}