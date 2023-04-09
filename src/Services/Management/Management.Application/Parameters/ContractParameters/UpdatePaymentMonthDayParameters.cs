using Newtonsoft.Json;

namespace Management.Application.Parameters.ContractParameters;

public class UpdatePaymentMonthDayParameters
{ 
    public int ContractId { get; init; }
    public int NewPaymentMonthDay { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public UpdatePaymentMonthDayParameters()
    { 
    }

    [JsonConstructor]
    public UpdatePaymentMonthDayParameters(int newPaymentMonthDay, int contractId)
    { 
        NewPaymentMonthDay = newPaymentMonthDay;
        ContractId = contractId;
    }
}