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

    public async Task<BonusProgramReader?> GetBonusProgramByIdAsync(Guid bonusProgramId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@bonusId", bonusProgramId);

        var result =
            await QueryAsync<BonusProgramReader>("program_getBaseData_S", parameters, CommandType.StoredProcedure);

        return result?.FirstOrDefault();
    }

    public async Task<BonusProgramReader?> GetBonusProgramDetailsByIdAsync(Guid bonusProgramId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@bonusId", bonusProgramId);

        var result =
            await QueryAsync<BonusProgramReader>("program_getWithDetails_S", parameters, CommandType.StoredProcedure);

        return result?.FirstOrDefault();
    }

    public async Task TaskGetBonusesByRecipientCode(string recipientCode)
    {
        //[TODO]
        var parameters = new DynamicParameters();
        parameters.Add("@recipientCode", recipientCode);

        var result =
            await QueryAsync<BonusProgramReader>("bonus_getByRecipientCode_S", parameters, CommandType.StoredProcedure);
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

        await ExecuteAsync("program_createNew_I", parameters, CommandType.StoredProcedure);
    }

    public async Task AddBonusRecipientAsync(BonusProgramReader bonusProgram)
    { 
        var bonusValue = bonusProgram.Bonuses!.Last();
        
        var parameters = new DynamicParameters();  
        
        parameters.Add("@bonusId", bonusProgram.Id);
        parameters.Add("@recipientCode", bonusValue.Recipient);
        parameters.Add("@groupBonus", bonusValue.GroupBonus);
        parameters.Add("@creator", bonusValue.Creator);
        parameters.Add("@canceled", bonusValue.Canceled);
        parameters.Add("@settled", bonusValue.Settled);
        parameters.Add("@created", bonusValue.Created);

        await ExecuteAsync("bonus_createNew_I", parameters, CommandType.StoredProcedure);
    }

    public async Task UpdateBonusRecipientAsync(BonusProgramReader bonusProgram)
    {
        var bonusValue = bonusProgram.Bonuses!.Last();

        var parameters = new DynamicParameters();

        parameters.Add("@bonusId", bonusValue.Id);   
        parameters.Add("@canceled", bonusValue.Canceled);
        parameters.Add("@settled", bonusValue.Settled); 

        await ExecuteAsync("bonus_finish_U", parameters, CommandType.StoredProcedure);
    }

    public async Task ClearOldBonusProgramsAsync()
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