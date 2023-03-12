using Newtonsoft.Json;

namespace Calculator.Application.Parameters.Bonus;

public class AddDepartmentToBonusParameters
{
    public Guid BonusProgram { get; init; }
    public string Department { get; init; }

    [JsonConstructor]
    public AddDepartmentToBonusParameters(Guid bonusProgram, string department)
    {
        BonusProgram = bonusProgram;
        Department = department;
    }
}