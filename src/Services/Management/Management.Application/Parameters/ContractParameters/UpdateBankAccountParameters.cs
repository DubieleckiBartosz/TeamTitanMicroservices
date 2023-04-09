using Newtonsoft.Json;

namespace Management.Application.Parameters.ContractParameters;

public class UpdateBankAccountParameters
{
    public int ContractId { get; init; }
    public string BankAccountNumber { get; init; }

    /// <summary>
    /// For tests
    /// </summary>
    public UpdateBankAccountParameters()
    {
    }

    [JsonConstructor]
    public UpdateBankAccountParameters(int contractId, string bankAccountNumber)
    {
        ContractId = contractId;
        BankAccountNumber = bankAccountNumber;
    }
}