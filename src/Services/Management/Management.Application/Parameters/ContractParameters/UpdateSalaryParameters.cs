using Newtonsoft.Json;

namespace Management.Application.Parameters.ContractParameters;

public class UpdateSalaryParameters
{
    public int EmployeeId { get; init; }
    public decimal NewSalary { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public UpdateSalaryParameters()
    {
    }

    [JsonConstructor]
    public UpdateSalaryParameters(int employeeId, decimal newSalary)
    {
        EmployeeId = employeeId;
        NewSalary = newSalary;
    }
}