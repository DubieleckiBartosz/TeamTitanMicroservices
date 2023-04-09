using Newtonsoft.Json;

namespace Management.Application.Parameters.ContractParameters;

public class UpdateFinancialDataParameters
{
    public int EmployeeId { get; init; }
    public decimal? HourlyRate { get; init; }
    public decimal? OvertimeRate { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public UpdateFinancialDataParameters()
    {
    }

    [JsonConstructor]
    public UpdateFinancialDataParameters(int employeeId, decimal? hourlyRate, decimal? overtimeRate)
    {
        EmployeeId = employeeId;
        HourlyRate = hourlyRate;
        OvertimeRate = overtimeRate;
    }
}