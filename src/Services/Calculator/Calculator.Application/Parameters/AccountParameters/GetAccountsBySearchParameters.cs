using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Newtonsoft.Json;
using Shared.Implementations.Search;
using Shared.Implementations.Search.SearchParameters;

namespace Calculator.Application.Parameters.AccountParameters;

public class GetAccountsBySearchParameters : BaseSearchQueryParameters, IFilterModel
{
    public Guid? AccountId { get; init; }
    public CountingType? CountingType { get; init; }
    public AccountStatus? AccountStatus { get; init; }
    public DateTime? ExpirationDateFrom { get; init; }
    public DateTime? ExpirationDateTo { get; init; }
    public string? ActivatedBy { get; init; }
    public string? DeactivatedBy { get; init; }
    public decimal? HourlyRateFrom { get; init; }
    public decimal? HourlyRateTo { get; init; }
    public int? SettlementDayMonth { get; init; }
    public decimal? BalanceFrom { get; init; }
    public decimal? BalanceTo { get; init; }
    public SortModelParameters Sort { get; set; }

    public GetAccountsBySearchParameters()
    {
    }

    [JsonConstructor]
    public GetAccountsBySearchParameters(Guid? accountId, CountingType? countingType, AccountStatus? accountStatus,
        DateTime? expirationDateFrom, DateTime? expirationDateTo, string? activatedBy, string? deactivatedBy,
        decimal? hourlyRateFrom, decimal? hourlyRateTo, int? settlementDayMonth, decimal? balanceFrom, decimal? balanceTo,
        SortModelParameters sort, int pageNumber, int pageSize)
    {
        AccountId = accountId;
        CountingType = countingType;
        AccountStatus = accountStatus;
        ExpirationDateFrom = expirationDateFrom;
        ExpirationDateTo = expirationDateTo;
        ActivatedBy = activatedBy;
        DeactivatedBy = deactivatedBy;
        HourlyRateFrom = hourlyRateFrom;
        HourlyRateTo = hourlyRateTo;
        SettlementDayMonth = settlementDayMonth;
        BalanceFrom = balanceFrom;
        BalanceTo = balanceTo;
        Sort = sort;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}