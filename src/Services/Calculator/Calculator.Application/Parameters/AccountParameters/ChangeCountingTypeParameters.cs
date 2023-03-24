using Calculator.Domain.Types;
using Newtonsoft.Json;

namespace Calculator.Application.Parameters.AccountParameters;

public class ChangeCountingTypeParameters
{
    public CountingType NewCountingType { get; init; }
    public Guid AccountId { get; init; }

    [JsonConstructor]
    public ChangeCountingTypeParameters(CountingType newCountingType, Guid accountId)
    {
        NewCountingType = newCountingType;
        AccountId = accountId;
    }
}