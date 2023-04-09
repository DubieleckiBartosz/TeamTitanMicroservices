using Management.Application.ValueTypes;
using Newtonsoft.Json;

namespace Management.Application.Parameters.ContractParameters;

public class UpdateSettlementTypeParameters
{
    public int EmployeeId { get; init; }
    public SettlementType NewSettlementType { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public UpdateSettlementTypeParameters()
    {
    }

    [JsonConstructor]
    public UpdateSettlementTypeParameters(int employeeId, SettlementType newSettlementType)
    {
        EmployeeId = employeeId;
        NewSettlementType = newSettlementType;
    }
}