using System.Data;
using Calculator.Application.Contracts;
using Calculator.Application.ReadModels.AccountReaders;
using Dapper;
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

    public async Task AddAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@departmentCode", accountReader.DepartmentCode);
        parameters.Add("@accountOwner", accountReader.AccountOwner);
        parameters.Add("@createdBy", accountReader.CreatedBy);
        parameters.Add("@accountId", accountReader.Id);

        await ExecuteAsync("account_createNew_I", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateStatusToDeactivateAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@countingType", accountReader.ActivatedBy);
        parameters.Add("@isActive", accountReader.IsActive);
        parameters.Add("@workDayHours", accountReader.WorkDayHours);
        parameters.Add("@overtimeRate", accountReader.OvertimeRate);
        parameters.Add("@hourlyRate", accountReader.HourlyRate);

        await ExecuteAsync("account_statusDeactivate_U", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateDataAsync(AccountReader accountReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@accountId", accountReader.Id);
        parameters.Add("@deactivatedBy", accountReader.DeactivatedBy);
        parameters.Add("@accountStatus", accountReader.AccountStatus);
        parameters.Add("@isActive", accountReader.IsActive);

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

        parameters.Add("@pieceworkProductId", productReader.PieceworkProductId);
        parameters.Add("@quantity", productReader.Quantity);
        parameters.Add("@currentPrice", productReader.CurrentPrice);
        parameters.Add("@accountId", accountReader.Id);

        await ExecuteAsync("product_createPieceworkProductItem_I", parameters, CommandType.StoredProcedure);
    }
}