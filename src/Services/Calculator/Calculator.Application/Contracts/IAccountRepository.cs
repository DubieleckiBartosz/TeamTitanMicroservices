using Calculator.Application.ReadModels.AccountReaders;

namespace Calculator.Application.Contracts;

public interface IAccountRepository
{
    Task<AccountReader> GetAccountByIdAsync(Guid accountId);
    Task<AccountReader> GetAccountByIdWithWorkDaysAsync(Guid accountId);
    Task<AccountReader> GetAccountByIdWithProductsAsync(Guid accountId);
    Task<AccountReader> GetAccountDetailsByIdAsync(Guid accountId);
    Task AddAsync(AccountReader accountReader);
    Task UpdateDataAsync(AccountReader accountReader);
    Task AddProductItemAsync(AccountReader accountReader);
    Task AddNewWorkDayAsync(AccountReader accountReader); 
}