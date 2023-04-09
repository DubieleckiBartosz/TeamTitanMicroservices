using Newtonsoft.Json;

namespace Management.Application.Parameters.ContractParameters;

public class UpdateSalaryParameters
{
    public int ContractId { get; init; } 
    public decimal NewSalary { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public UpdateSalaryParameters()
    { 
    }

    [JsonConstructor]
    public UpdateSalaryParameters(decimal newSalary, int contractId)
    {
        NewSalary = newSalary;
        ContractId = contractId;
    }
}