using Newtonsoft.Json;

namespace Calculator.Application.Parameters.Bonus;

public class AddRecipientToBonusParameters
{
    public string Account { get; init; }
    public Guid BonusProgram { get; init; }
    public bool BonusGroup { get; init; }

    [JsonConstructor]
    public AddRecipientToBonusParameters(string account, Guid bonusProgram, bool bonusGroup)
    {
        Account = account;
        BonusProgram = bonusProgram;
        BonusGroup = bonusGroup;
    }
}