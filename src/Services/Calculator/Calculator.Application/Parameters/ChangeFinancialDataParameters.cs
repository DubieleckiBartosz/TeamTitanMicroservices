using Newtonsoft.Json;

namespace Calculator.Application.Parameters;

public class ChangeFinancialDataParameters
{
    public decimal? OvertimeRate { get; init; }
    public decimal? HourlyRate { get; init; } 
    public Guid AccountId { get; init; }

    [JsonConstructor]
    public ChangeFinancialDataParameters(decimal? overtimeRate, decimal? hourlyRate, Guid accountId)
    {
        OvertimeRate = overtimeRate;
        HourlyRate = hourlyRate;
        AccountId = accountId;
    }
}