using Calculator.Application.ReadModels.AccountReaders;

namespace Calculator.Application.Contracts;

public interface IAccountRepository
{
    Task<AccountReader?> GetAccountsBySearchAsync();
    Task<AccountReader?> GetAccountByIdAsync(Guid accountId);
    Task<AccountReader?> GetAccountByIdWithWorkDaysAsync(Guid accountId);
    Task<AccountReader?> GetAccountByIdWithProductsAsync(Guid accountId);
    Task<AccountReader?> GetAccountByIdWithBonusesAsync(Guid accountId);
    Task<AccountReader?> GetAccountDetailsByIdAsync(Guid accountId);
    Task AddAsync(AccountReader accountReader);
    Task AddBonusAsync(AccountReader account);
    Task UpdateDataAsync(AccountReader accountReader);
    Task UpdateFinancialDataAsync(AccountReader accountReader);
    Task UpdateBonusAccountAsync(BonusReader bonusValue, AccountReader account);
    Task UpdateStatusToDeactivateAsync(AccountReader accountReader);
    Task UpdateCountingTypeAsync(AccountReader accountReader);
    Task UpdateWorkDayHoursAsync(AccountReader accountReader);
    Task UpdateHourlyRateAsync(AccountReader accountReader);
    Task UpdateOvertimeRateAsync(AccountReader accountReader);
    Task UpdateStatusToActiveAsync(AccountReader accountReader);  
    Task AddProductItemAsync(AccountReader accountReader);
    Task AddNewWorkDayAsync(AccountReader accountReader); 
}