using Calculator.Application.Features.Account.ViewModels;
using Calculator.Application.Parameters;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Search;
using Shared.Implementations.Tools;

namespace Calculator.Application.Features.Account.Queries.GetAccountsBySearch;

public record GetAccountsBySearchQuery(Guid? AccountId, CountingType? CountingType, AccountStatus? AccountStatus,
    DateTime? ExpirationDateFrom, DateTime? ExpirationDateTo, string? ActivatedBy, string? DeactivatedBy, decimal? HourlyRateFrom,
    decimal? HourlyRateTo, int? SettlementDayMonth, decimal? BalanceFrom, decimal? BalanceTo,
    SortModel Sort, int PageNumber, int PageSize) : IQuery<AccountSearchViewModel>
{
    public static GetAccountsBySearchQuery Create(GetAccountsBySearchParameters parameters)
    {
        var accountId = parameters.AccountId;
        var countingType = parameters.CountingType; 
        var accountStatus = parameters.AccountStatus;
        var expirationDateFrom = parameters.ExpirationDateFrom;
        var expirationDateTo = parameters.ExpirationDateTo;
        var activatedBy = parameters.ActivatedBy;
        var deactivatedBy = parameters.DeactivatedBy;
        var hourlyRateFrom = parameters.HourlyRateFrom;
        var hourlyRateTo = parameters.HourlyRateTo;
        var settlementDayMonth = parameters.SettlementDayMonth;
        var balanceFrom = parameters.BalanceFrom;
        var balanceTo = parameters.BalanceTo;
        var sort = parameters.CheckOrAssignSortModel();
        var pageNumber = parameters.PageNumber;
        var pageSize = parameters.PageSize;

        return new GetAccountsBySearchQuery(accountId, countingType, accountStatus,
            expirationDateFrom, expirationDateTo, activatedBy, deactivatedBy, hourlyRateFrom,
            hourlyRateTo, settlementDayMonth, balanceFrom, balanceTo, sort, pageNumber, pageSize);
    }
}