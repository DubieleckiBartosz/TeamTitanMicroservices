using Newtonsoft.Json;

namespace Management.Application.Parameters.ContractParameters;

public class UpdatePaymentMonthDayParameters
{
    public int EmployeeId { get; init; }
    public int NewPaymentMonthDay { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public UpdatePaymentMonthDayParameters()
    {
    }

    [JsonConstructor]
    public UpdatePaymentMonthDayParameters(int employeeId, int newPaymentMonthDay)
    {
        EmployeeId = employeeId;
        NewPaymentMonthDay = newPaymentMonthDay;
    }
}