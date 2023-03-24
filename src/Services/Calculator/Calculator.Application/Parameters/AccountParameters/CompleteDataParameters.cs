using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Newtonsoft.Json;

namespace Calculator.Application.Parameters.AccountParameters;

public class CompleteDataParameters
{
    public CountingType CountingType { get; init; }
    public AccountStatus Status { get; init; }
    public int WorkDayHours { get; init; }
    public int SettlementDayMonth { get; init; }
    public Guid AccountId { get; init; }
    public DateTime? ExpirationDate { get; init; }

    [JsonConstructor]
    public CompleteDataParameters(CountingType countingType, AccountStatus status, int workDayHours,
        int settlementDayMonth, Guid accountId, DateTime? expirationDate)
    {
        CountingType = countingType;
        Status = status;
        WorkDayHours = workDayHours;
        SettlementDayMonth = settlementDayMonth;
        AccountId = accountId;
        ExpirationDate = expirationDate;
    }
}