using Management.Application.ValueTypes;
using Newtonsoft.Json;

namespace Management.Application.Parameters.ContractParameters;

public class UpdateSettlementTypeParameters
{
    public int ContractId { get; init; } 
    public SettlementType NewSettlementType { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public UpdateSettlementTypeParameters()
    { 
    }

    [JsonConstructor]
    public UpdateSettlementTypeParameters(SettlementType newSettlementType, int contractId)
    {
        NewSettlementType = newSettlementType;
        ContractId = contractId;
    }
}