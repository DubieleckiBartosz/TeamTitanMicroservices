using Newtonsoft.Json;

namespace Management.Application.Parameters.ContractParameters;

public class UpdateDayHoursParameters
{
    public int ContractId { get; init; }
    public int NumberHoursPerDay { get; init; }

    public UpdateDayHoursParameters()
    {
    }

    [JsonConstructor]
    public UpdateDayHoursParameters(int contractId, int numberHoursPerDay)
    {
        ContractId = contractId;
        NumberHoursPerDay = numberHoursPerDay;
    }
}