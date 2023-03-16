using System.Data;
using Calculator.Application.Contracts;
using Calculator.Application.ReadModels.AccountReaders;
using Calculator.Infrastructure.DataAccessObjects;
using Dapper;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Calculator.Infrastructure.Repositories;

public class AccountRepository : BaseRepository<AccountRepository>, IAccountRepository
{
    public AccountRepository(string dbConnection, ILoggerManager<AccountRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }

    public async Task<AccountReader?> GetAccountsBySearchAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<AccountReader?> GetAccountByIdAsync(Guid accountId)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountId);

        var result = await QueryAsync<AccountDao>("account_getById_S", parameters, CommandType.StoredProcedure);

        return result?.FirstOrDefault()?.Map();
    }

    public async Task<AccountReader?> GetAccountByIdWithBonusesAsync(Guid accountId)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountId);

        var dict = new Dictionary<Guid, AccountDao>();

        var result = (await QueryAsync<AccountDao, BonusDao, AccountDao>(
            "account_getByIdWithBonuses_S", (a, b) =>
            {
                if (!dict.TryGetValue(a.Id, out var value))
                {
                    value = a;
                    dict.Add(a.Id, value);
                }

                value.Bonuses.Add(b);

                return value;
            }, "Id,Id", parameters, CommandType.StoredProcedure)).FirstOrDefault(); 

        return result?.Map();
    } 
     
    public async Task<AccountReader?> GetAccountByIdWithWorkDaysAsync(Guid accountId)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountId);

        var dict = new Dictionary<Guid, AccountDao>();
        var result = (await QueryAsync<AccountDao, WorkDayDao, AccountDao>(
            "account_getByIdWithWorkDays_s", (a, w) =>
            {
                if (!dict.TryGetValue(a.Id, out var value))
                {
                    value = a;
                    dict.Add(a.Id, value);
                }

                value.WorkDays.Add(w);

                return value;
            }, "Id,Id", parameters, CommandType.StoredProcedure)).FirstOrDefault();

        return result?.Map();
    }

    public async Task<AccountReader?> GetAccountByIdWithProductsAsync(Guid accountId)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountId);

        var dict = new Dictionary<Guid, AccountDao>();
        var result = (await QueryAsync<AccountDao, ProductItemDao, AccountDao>(
            "account_getByIdWithProducts_s", (a, p) =>
            {
                if (!dict.TryGetValue(a.Id, out var value))
                {
                    value = a;
                    dict.Add(a.Id, value);
                }

                value.ProductItems.Add(p);

                return value;
            }, "Id,Id", parameters, CommandType.StoredProcedure)).FirstOrDefault();

        return result?.Map();
    }

    public async Task<AccountReader?> GetAccountDetailsByIdAsync(Guid accountId)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountId);

        var dict = new Dictionary<Guid, AccountDao>();
        var result = (await QueryAsync<AccountDao, WorkDayDao, ProductItemDao, AccountDao>(
            "account_getDetailsById_s", (a, w, p) =>
            {
                if (!dict.TryGetValue(a.Id, out var value))
                {
                    value = a;
                    dict.Add(a.Id, value);
                }

                value.WorkDays.Add(w);
                value.ProductItems.Add(p);

                return value;
            }, "Id,Id,Id", parameters, CommandType.StoredProcedure)).FirstOrDefault();

        return result?.Map();
    }

    public async Task AddAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@departmentCode", accountReader.DepartmentCode);
        parameters.Add("@accountOwner", accountReader.AccountOwner);
        parameters.Add("@createdBy", accountReader.CreatedBy); 
        parameters.Add("@isActive", accountReader.IsActive); 

        await ExecuteAsync("account_createNew_I", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateStatusToDeactivateAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id); 
        parameters.Add("@isActive", accountReader.IsActive);
        parameters.Add("@deactivatedBy", accountReader.DeactivatedBy);
        parameters.Add("@accountStatus", (int) accountReader.AccountStatus); 

        await ExecuteAsync("account_statusDeactivate_U", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateDataAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters(); 

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@countingType", (int) accountReader.CountingType);
        parameters.Add("@status", (int) accountReader.AccountStatus);
        parameters.Add("@workDayHours", accountReader.WorkDayHours);
        parameters.Add("@overtimeRate", accountReader.OvertimeRate);
        parameters.Add("@hourlyRate", accountReader.HourlyRate);

        await ExecuteAsync("account_completeData_U", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateCountingTypeAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@newCountingType", accountReader.CountingType);

        await ExecuteAsync("account_newCountingType_U", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateWorkDayHoursAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@newWorkDayHours", accountReader.WorkDayHours);

        await ExecuteAsync("account_newWorkDayHours_U", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateHourlyRateAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@newHourlyRate", accountReader.HourlyRate);

        await ExecuteAsync("account_newHourlyRate_U", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateOvertimeRateAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@newOvertimeRate", accountReader.OvertimeRate);

        await ExecuteAsync("account_newOvertimeRate_U", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateStatusToActiveAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@activatedBy", accountReader.ActivatedBy);
        parameters.Add("@accountStatus", accountReader.AccountStatus);
        parameters.Add("@isActive", accountReader.IsActive);

        await ExecuteAsync("account_statusActive_U", parameters, CommandType.StoredProcedure);
    }

    public async Task AddNewWorkDayAsync(AccountReader accountReader)
    {
        var workDayReader = accountReader.GetLastWorkDay()!;
        var parameters = new DynamicParameters();

        parameters.Add("@balance", accountReader.Balance);
        parameters.Add("@date", workDayReader.Date);
        parameters.Add("@hoursWorked", workDayReader.HoursWorked);
        parameters.Add("@overtime", workDayReader.Overtime);
        parameters.Add("@isDayOff", workDayReader.IsDayOff);
        parameters.Add("@createdBy", workDayReader.CreatedBy);
        parameters.Add("@accountId", accountReader.Id);

        await ExecuteAsync("day_createWorkDay_I", parameters, CommandType.StoredProcedure);
    }

    public async Task AddProductItemAsync(AccountReader accountReader)
    { 
        var productReader = accountReader.GetLastProductItem()!;
        var parameters = new DynamicParameters();

        parameters.Add("@balance", accountReader.Balance);
        parameters.Add("@pieceworkProductId", productReader.PieceworkProductId);
        parameters.Add("@quantity", productReader.Quantity);
        parameters.Add("@currentPrice", productReader.CurrentPrice);
        parameters.Add("@date", productReader.Date);
        parameters.Add("@isConsidered", productReader.IsConsidered);
        parameters.Add("@accountId", accountReader.Id);

        await ExecuteAsync("product_createPieceworkProductItem_I", parameters, CommandType.StoredProcedure);
    }

    public async Task AddBonusAsync(AccountReader account)
    {
        var bonusValue = account.Bonuses!.Last();

        var parameters = new DynamicParameters();

        parameters.Add("@accountId", account.Id);
        parameters.Add("@balance", account.Balance);
        parameters.Add("@bonusCode", bonusValue.BonusCode);
        parameters.Add("@creator", bonusValue.Creator);
        parameters.Add("@canceled", bonusValue.Canceled);
        parameters.Add("@settled", bonusValue.Settled);
        parameters.Add("@created", bonusValue.Created);

        await ExecuteAsync("account_createNewBonus_I", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateBonusAccountAsync(BonusReader bonusValue, AccountReader account)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", account.Id);
        parameters.Add("@balance", account.Balance);
        parameters.Add("@bonusCode", bonusValue.BonusCode);
        parameters.Add("@canceled", bonusValue.Canceled);
        parameters.Add("@settled", bonusValue.Settled);

        await ExecuteAsync("account_finishBonusLife_U", parameters, CommandType.StoredProcedure);
    }

    public async Task ClearOldBonusesAsync()
    {
        var parameters = new DynamicParameters();
        await ExecuteAsync("account_bonusClear_D", parameters, CommandType.StoredProcedure);
    }

    public async Task ClearSettledAndCanceledBonusesAsync(DateTime dateStartCleaning)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@dateStartCleaning", dateStartCleaning);


        await ExecuteAsync("account_clearSettledAndCanceledBonuses_D", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateAccountProductItemsAsync(AccountReader accountReader)
    {
        var productReader = accountReader.ProductItems.Select(_ => _.PieceworkProductId).ToList();
        var parameters = new DynamicParameters();

        //[TODO]

        await ExecuteAsync("product_productItemsAsConsidered_U", parameters, CommandType.StoredProcedure);
    }
}