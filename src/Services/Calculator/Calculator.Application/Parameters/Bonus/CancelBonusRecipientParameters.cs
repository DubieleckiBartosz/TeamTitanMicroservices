using Newtonsoft.Json;

namespace Calculator.Application.Parameters.Bonus;

public class CancelBonusRecipientParameters
{
    [JsonConstructor]
    public CancelBonusRecipientParameters(Guid bonusProgram, string recipientCode, string bonusCode)
    {
        BonusProgram = bonusProgram;
        RecipientCode = recipientCode;
        BonusCode = bonusCode;
    }

    public Guid BonusProgram { get; init; } 
    public string RecipientCode { get; init; }
    public string BonusCode { get; init; } 
}