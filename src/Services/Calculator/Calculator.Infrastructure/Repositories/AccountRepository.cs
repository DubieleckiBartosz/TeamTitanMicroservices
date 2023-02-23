using Calculator.Application.Contracts;
using Calculator.Application.ReadModels.AccountReaders;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Calculator.Infrastructure.Repositories;

public class AccountRepository : BaseRepository<AccountRepository>, IAccountRepository
{
    public AccountRepository(string dbConnection, ILoggerManager<AccountRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }

    public Task<AccountReader> GetAccountByIdAsync(Guid accountId)
    {
        throw new NotImplementedException();
    }

    public Task<AccountReader> GetAccountByIdWithWorkDaysAsync(Guid accountId)
    {
        throw new NotImplementedException();
    }

    public Task<AccountReader> GetAccountByIdWithProductsAsync(Guid accountId)
    {
        throw new NotImplementedException();
    }

    public Task<AccountReader> GetAccountDetailsByIdAsync(Guid accountId)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(AccountReader accountReader)
    {
        throw new NotImplementedException();
    }

    public Task UpdateDataAsync(AccountReader accountReader)
    {
        throw new NotImplementedException();
    }

    public Task AddNewWorkDayAsync(AccountReader accountReader)
    {
        throw new NotImplementedException();
    }
    public Task AddProductItemAsync(AccountReader accountReader)
    {
        throw new NotImplementedException();
    } 
}