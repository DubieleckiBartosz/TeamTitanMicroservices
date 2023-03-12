using Calculator.Application.Contracts;
using Calculator.Application.ReadModels.BonusReaders;
using Dapper;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;
using System.Data;

namespace Calculator.Infrastructure.Repositories;

public class BonusProgramRepository : BaseRepository<BonusProgramRepository>, IBonusProgramRepository
{
    public BonusProgramRepository(string dbConnection, ILoggerManager<BonusProgramRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }

    public Task<BonusProgramReader> GetBonusProgramWithDepartmentsByIdAsync(Guid bonusProgramId)
    {
        throw new NotImplementedException();
    }

    public Task<BonusProgramReader> GetBonusProgramWithAccountsByIdAsync(Guid bonusProgramId)
    {
        throw new NotImplementedException();
    }

    public Task<BonusProgramReader> GetBonusProgramDetailsByIdAsync(Guid bonusProgramId)
    {
        throw new NotImplementedException();
    }

    public async Task AddNewBonusProgramAsync(BonusProgramReader bonusProgram)
    {
        var parameters = new DynamicParameters();
     
        parameters.Add("@bonusId", bonusProgram.Id);
        parameters.Add("@bonusAmount", bonusProgram.BonusAmount);
        parameters.Add("@createdBy", bonusProgram.CreatedBy);
        parameters.Add("@companyCode", bonusProgram.CompanyCode);
        parameters.Add("@expires", bonusProgram.Expires);
        parameters.Add("@reason", bonusProgram.Reason);

        await ExecuteAsync("bonus_createNew_I", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateOrInsertBonusProgramDepartmentAsync(BonusProgramReader bonusProgram)
    {
        var departmentCode = bonusProgram.Departments!.Keys.Last();
        var value = bonusProgram.Departments[departmentCode];
        var count = value.Count;
        var bonusValue = value.Bonuses.Last();
        
        var parameters = new DynamicParameters();  
        
        parameters.Add("@bonusId", bonusProgram.Id);
        parameters.Add("@department", departmentCode);
        parameters.Add("@count", count);
        parameters.Add("@creator", bonusValue.Creator);
        parameters.Add("@canceled", bonusValue.Canceled);
        parameters.Add("@settled", bonusValue.Settled);
        parameters.Add("@created", bonusValue.Created);

        await ExecuteAsync("bonus_department_U", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateBonusProgramDepartmentAsync(BonusProgramReader bonusProgram)
    {
        var parameters = new DynamicParameters();
        

        await ExecuteAsync("bonus_department_U", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateOrInsertBonusProgramAccountAsync(BonusProgramReader bonusProgram)
    {
        var accountCode = bonusProgram.Accounts!.Keys.Last();
        var value = bonusProgram.Accounts[accountCode];
        var count = value.Count;
        var bonusValue = value.Bonuses.Last();

        var parameters = new DynamicParameters();

        parameters.Add("@bonusId", bonusProgram.Id);
        parameters.Add("@account", accountCode);
        parameters.Add("@count", count);
        parameters.Add("@creator", bonusValue.Creator);
        parameters.Add("@canceled", bonusValue.Canceled);
        parameters.Add("@settled", bonusValue.Settled);
        parameters.Add("@created", bonusValue.Created);

        await ExecuteAsync("bonus_account_U", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateBonusProgramAccount(string account, int count, )
    { 
        var parameters = new DynamicParameters();
        

        await ExecuteAsync("bonus_account_U", parameters, CommandType.StoredProcedure);
    }

    public async Task ClearOldBonusProgramsAsync(List<Guid> programs)
    {
        var parameters = new DynamicParameters();
        await ExecuteAsync("bonus_clear_D", parameters, CommandType.StoredProcedure);
    }

    public async Task ClearSettledAndCanceledBonusesAsync(DateTime dateStartCleaning)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@dateStartCleaning", dateStartCleaning);


        await ExecuteAsync("bonus_clearSettledAndCanceledBonuses_D", parameters, CommandType.StoredProcedure);
    }
}