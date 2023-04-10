using System.Data;
using Calculator.Application.Contracts.Repositories;
using Calculator.Application.ReadModels.AccountReaders;
using Calculator.Domain.Statuses;
using Calculator.Domain.Types;
using Calculator.Infrastructure.DataAccessObjects.AccountDataAccessObjects;
using Dapper;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;
using Shared.Implementations.Search;

namespace Calculator.Infrastructure.Repositories;

public class AccountRepository : BaseRepository<AccountRepository>, IAccountRepository
{
    public AccountRepository(string dbConnection, ILoggerManager<AccountRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }

    public async Task<ResponseSearchList<AccountReader>?> GetAccountsBySearchAsync(
        Guid? accountId, CountingType? countingType, AccountStatus? accountStatus,
        DateTime? expirationDateFrom, DateTime? expirationDateTo, string? activatedBy, string? deactivatedBy, decimal? hourlyRateFrom,
        decimal? hourlyRateTo, int? settlementDayMonth, decimal? balanceFrom, decimal? balanceTo,
        string type, string name, int pageNumber, int pageSize, string companyCode)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountId);
        parameters.Add("@countingType", countingType);
        parameters.Add("@accountStatus", accountStatus);
        parameters.Add("@expirationDateFrom", expirationDateFrom);
        parameters.Add("@expirationDateTo", expirationDateTo);
        parameters.Add("@activatedBy", activatedBy);
        parameters.Add("@deactivatedBy", deactivatedBy);
        parameters.Add("@hourlyRateFrom", hourlyRateFrom);
        parameters.Add("@hourlyRateTo", hourlyRateTo);
        parameters.Add("@settlementDayMonth", settlementDayMonth);
        parameters.Add("@balanceFrom", balanceFrom);
        parameters.Add("@balanceTo", balanceTo);
        parameters.Add("@type", type);
        parameters.Add("@name", name);
        parameters.Add("@pageNumber", pageNumber);
        parameters.Add("@pageSize", pageSize);
        parameters.Add("@companyCode", companyCode);

        var result =
            (await QueryAsync<AccountSearchDao>("account_getBySearch_S", parameters, CommandType.StoredProcedure))
            ?.ToList();

        var totalCount = result?.FirstOrDefault()?.TotalCount;
        var data = result?.Select(_ => _.Map()).ToList();

        return ResponseSearchList<AccountReader>.Create(data, totalCount ?? 0); 
    }

    public async Task<AccountReader?> GetAccountByIdAsync(Guid accountId)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountId);

        var result = await QueryAsync<AccountDao>("account_getById_S", parameters, CommandType.StoredProcedure);

        return result?.FirstOrDefault()?.Map();
    }

    public async Task<AccountReader?> GetAccountWithProductsAndBonusesByIdAsync(Guid accountId, DateTime? startDate, DateTime? toDate)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountId);
        parameters.Add("@startDate", startDate);
        parameters.Add("@toDate", toDate);

        var dict = new Dictionary<Guid, AccountDao>();
        var result = await QueryAsync<AccountDao, ProductItemDao?, BonusDao?, AccountDao>(
            "account_getWithProductsAndBonusesById_S", (a, p, b) =>
            {
                if (!dict.TryGetValue(a.Id, out var value))
                {
                    value = a;
                    dict.Add(a.Id, value);
                }

                if (p != null)
                {
                    value.ProductItems.Add(p);
                }


                if (b != null)
                {
                    value.Bonuses.Add(b);
                }

                return value;
        }, splitOn: "Id,Id,Id", parameters, CommandType.StoredProcedure);

        return result?.FirstOrDefault()?.Map();
    }

    public async Task<AccountReader?> GetAccountByIdWithBonusesAsync(Guid accountId)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountId);

        var dict = new Dictionary<Guid, AccountDao>();

        var result = (await QueryAsync<AccountDao, BonusDao?, AccountDao>(
            "account_getByIdWithBonuses_S", (a, b) =>
            {
                if (!dict.TryGetValue(a.Id, out var value))
                {
                    value = a;
                    dict.Add(a.Id, value);
                }

                if (b != null)
                {
                    value.Bonuses.Add(b);
                }

                return value;
            }, "Id,Id", parameters, CommandType.StoredProcedure)).FirstOrDefault(); 

        return result?.Map();
    } 
     
    public async Task<AccountReader?> GetAccountByIdWithWorkDaysAsync(Guid accountId)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountId);

        var dict = new Dictionary<Guid, AccountDao>();
        var result = (await QueryAsync<AccountDao, WorkDayDao?, AccountDao>(
            "account_getByIdWithWorkDays_s", (a, w) =>
            {
                if (!dict.TryGetValue(a.Id, out var value))
                {
                    value = a;
                    dict.Add(a.Id, value);
                }

                if (w != null)
                {
                    value.WorkDays.Add(w);
                }

                return value;
            }, "Id,Id", parameters, CommandType.StoredProcedure)).FirstOrDefault();

        return result?.Map();
    }

    public async Task<AccountReader?> GetAccountByIdWithProductsAsync(Guid accountId)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountId);

        var dict = new Dictionary<Guid, AccountDao>();
        var result = (await QueryAsync<AccountDao, ProductItemDao?, AccountDao>(
            "account_getByIdWithProducts_s", (a, p) =>
            {
                if (!dict.TryGetValue(a.Id, out var value))
                {
                    value = a;
                    dict.Add(a.Id, value);
                }

                if (p != null)
                {
                    value.ProductItems.Add(p);
                }

                return value;
            }, "Id,Id", parameters, CommandType.StoredProcedure)).FirstOrDefault();

        return result?.Map();
    }

    public async Task<AccountReader?> GetAccountDetailsByIdAsync(Guid accountId)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountId);

        var dict = new Dictionary<Guid, AccountDao>();
        var result = (await QueryAsync<AccountDao, WorkDayDao?, ProductItemDao?, AccountDao>(
            "account_getDetailsById_s", (a, w, p) =>
            {
                if (!dict.TryGetValue(a.Id, out var value))
                {
                    value = a;
                    dict.Add(a.Id, value);
                }

                if (w != null)
                {
                    value.WorkDays.Add(w);
                }

                if (p != null)
                {
                    value.ProductItems.Add(p);
                }

                return value;
            }, "Id,Id,Id", parameters, CommandType.StoredProcedure)).FirstOrDefault();

        return result?.Map();
    }

    public async Task AddAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@companyCode", accountReader.CompanyCode);
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
        parameters.Add("@expirationDate", accountReader.ExpirationDate);
        parameters.Add("@countingType", (int) accountReader.CountingType);
        parameters.Add("@status", (int) accountReader.AccountStatus);
        parameters.Add("@workDayHours", accountReader.WorkDayHours);
        parameters.Add("@settlementDayMonth", accountReader.SettlementDayMonth);
        parameters.Add("@payoutAmount", accountReader.PayoutAmount);
         
        await ExecuteAsync("account_updateData_U", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateFinancialDataAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@overtimeRate", accountReader.OvertimeRate);
        parameters.Add("@hourlyRate", accountReader.HourlyRate);

        await ExecuteAsync("account_financialData_U", parameters, CommandType.StoredProcedure);
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

    public async Task UpdateStatusToActiveAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@activatedBy", accountReader.ActivatedBy);
        parameters.Add("@accountStatus", accountReader.AccountStatus);
        parameters.Add("@isActive", accountReader.IsActive);

        await ExecuteAsync("account_statusActive_U", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateSettlementDayMonthAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@newSettlementDayMonth", accountReader.SettlementDayMonth); 

        await ExecuteAsync("account_settlementDayMonth_U", parameters, CommandType.StoredProcedure);
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

    public async Task AddSettlementAsync(AccountReader accountReader)
    {
        var settlement = accountReader.Settlements.Last();

        /*
             TVP has a maximum size of 2 MB (as is the default for SQL Server 2016 and later), 
             then the maximum number of GUIDs that can be stored in the TVP is:

             2 MB = 2,048 KB = 2,097,152 bytes
             2,097,152 bytes / 16 bytes per GUID = 131,072 GUIDs
        */

        var toBonusTableType = accountReader.Bonuses?.Select(_ => _.Id).ToList();
        var toProductTableType = accountReader.ProductItems?.Select(_ => _.PieceworkProductId).ToList();
        var bonusTable = new DataTable();
        var productTable = new DataTable();

        bonusTable.Columns.Add(new DataColumn("Id", typeof(Guid)));
        productTable.Columns.Add(new DataColumn("Id", typeof(Guid)));

        toBonusTableType?.ForEach(_ => bonusTable.Rows.Add(_));
        toProductTableType?.ForEach(_ => productTable.Rows.Add(_));


        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@from", settlement.From);
        parameters.Add("@to", settlement.To);
        parameters.Add("@value", settlement.Value);

        parameters.Add("@bonuses",
            bonusTable.AsTableValuedParameter("ConsiderationTableType"));

        parameters.Add("@products",
            productTable.AsTableValuedParameter("ConsiderationTableType"));

        await ExecuteAsync("settlement_createSettlement_I", parameters, CommandType.StoredProcedure);  
    }


    public async Task AddBonusAsync(AccountReader account)
    {
        var bonusValue = account.Bonuses!.Last();

        var parameters = new DynamicParameters();

        parameters.Add("@accountId", account.Id);
        parameters.Add("@balance", account.Balance);
        parameters.Add("@amount", bonusValue.Amount);
        parameters.Add("@bonusCode", bonusValue.BonusCode);
        parameters.Add("@creator", bonusValue.Creator);
        parameters.Add("@canceled", bonusValue.Canceled);
        parameters.Add("@settled", bonusValue.Settled);
        parameters.Add("@created", bonusValue.Created);

        await ExecuteAsync("bonus_createNew_I", parameters, CommandType.StoredProcedure);
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