using Newtonsoft.Json;

namespace Management.Application.Parameters.ContractParameters;

public class UpdateFinancialDataParameters
{ 
    public int ContractId { get; init; }
    public decimal? Salary { get; init; }
    public decimal? HourlyRate { get; init; }
    public decimal? OvertimeRate { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public UpdateFinancialDataParameters()
    { 
    }

    [JsonConstructor]
    public UpdateFinancialDataParameters(decimal? hourlyRate, decimal? overtimeRate, int contractId, decimal? salary)
    { 
        HourlyRate = hourlyRate;
        OvertimeRate = overtimeRate;
        ContractId = contractId;
        Salary = salary;
    }
}