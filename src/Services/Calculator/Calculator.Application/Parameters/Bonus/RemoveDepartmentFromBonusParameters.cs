using Newtonsoft.Json;

namespace Calculator.Application.Parameters.Bonus;

public class RemoveDepartmentFromBonusParameters
{
    public Guid BonusProgram { get; init; } 
    public string DepartmentCode { get; init; }

    [JsonConstructor]
    public RemoveDepartmentFromBonusParameters(Guid bonusProgram, string departmentCode)
    {
        BonusProgram = bonusProgram;
        DepartmentCode = departmentCode;
    }
}