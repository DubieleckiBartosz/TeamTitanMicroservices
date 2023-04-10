using Calculator.Application.ReadModels.AccountReaders;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Shared.Implementations.Search;

namespace Calculator.Application.Contracts.Repositories;

public interface IAccountRepository
{
    Task<ResponseSearchList<AccountReader>?> GetAccountsBySearchAsync(Guid? accountId, CountingType? countingType, AccountStatus? accountStatus,
        DateTime? expirationDate, DateTime? expirationDateTo, string? activatedBy, string? deactivatedBy, decimal? hourlyRateFrom,
        decimal? hourlyRateTo, int? settlementDayMonth, decimal? balanceFrom, decimal? balanceTo,
        string type, string name, int pageNumber, int pageSize, string companyCode);
    Task<AccountReader?> GetAccountByIdAsync(Guid accountId);
    Task<AccountReader?> GetAccountByIdWithWorkDaysAsync(Guid accountId);
    Task<AccountReader?> GetAccountByIdWithProductsAsync(Guid accountId);
    Task<AccountReader?> GetAccountByIdWithBonusesAsync(Guid accountId);
    Task<AccountReader?> GetAccountDetailsByIdAsync(Guid accountId);
    Task<AccountReader?> GetAccountWithProductsAndBonusesByIdAsync(Guid accountId, DateTime? startDate = null,
        DateTime? toDate = null);
    Task AddAsync(AccountReader accountReader);
    Task AddBonusAsync(AccountReader account);
    Task AddProductItemAsync(AccountReader accountReader);
    Task AddNewWorkDayAsync(AccountReader accountReader);
    Task AddSettlementAsync(AccountReader accountReader);
    Task UpdateDataAsync(AccountReader accountReader);
    Task UpdateFinancialDataAsync(AccountReader accountReader);
    Task UpdateBonusAccountAsync(BonusReader bonusValue, AccountReader account);
    Task UpdateStatusToDeactivateAsync(AccountReader accountReader);
    Task UpdateCountingTypeAsync(AccountReader accountReader);
    Task UpdateWorkDayHoursAsync(AccountReader accountReader); 
    Task UpdateStatusToActiveAsync(AccountReader accountReader);
    Task UpdateSettlementDayMonthAsync(AccountReader accountReader);
}