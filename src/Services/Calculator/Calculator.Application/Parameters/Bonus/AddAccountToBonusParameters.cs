using Newtonsoft.Json;

namespace Calculator.Application.Parameters.Bonus;

public class AddAccountToBonusParameters
{
    public string Account { get; init; }
    public Guid BonusProgram { get; init; }

    [JsonConstructor]
    public AddAccountToBonusParameters(string account, Guid bonusProgram)
    {
        Account = account;
        BonusProgram = bonusProgram;
    }
}