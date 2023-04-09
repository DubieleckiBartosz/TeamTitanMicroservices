using Newtonsoft.Json;

namespace Management.Application.Parameters.ContractParameters;

public class UpdateHourlyRatesParameters
{ 
    public int ContractId { get; init; }
    public decimal? HourlyRate { get; init; }
    public decimal? OvertimeRate { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public UpdateHourlyRatesParameters()
    {
    }

    [JsonConstructor]
    public UpdateHourlyRatesParameters(decimal? hourlyRate, decimal? overtimeRate, int contractId)
    { 
        HourlyRate = hourlyRate;
        OvertimeRate = overtimeRate;
        ContractId = contractId;
    }
}