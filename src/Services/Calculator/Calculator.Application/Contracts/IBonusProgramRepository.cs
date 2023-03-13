using Calculator.Application.ReadModels.BonusReaders;

namespace Calculator.Application.Contracts;

public interface IBonusProgramRepository
{
    Task<BonusProgramReader?> GetBonusProgramByIdAsync(Guid bonusProgramId); 
    Task<BonusProgramReader?> GetBonusProgramDetailsByIdAsync(Guid bonusProgramId);
    Task AddNewBonusProgramAsync(BonusProgramReader bonusProgram);
    Task AddBonusRecipientAsync(BonusProgramReader bonusProgram);
    Task UpdateBonusRecipientAsync(BonusProgramReader bonusProgram);
    Task ClearOldBonusProgramsAsync();
    Task ClearSettledAndCanceledBonusesAsync(DateTime dateStartCleaning);
}