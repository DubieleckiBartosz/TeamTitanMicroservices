using Calculator.Application.ReadModels.BonusReaders;

namespace Calculator.Application.Contracts;

public interface IBonusProgramRepository
{
    Task<BonusProgramReader> GetBonusProgramWithDepartmentsByIdAsync(Guid bonusProgramId);
    Task<BonusProgramReader> GetBonusProgramWithAccountsByIdAsync(Guid bonusProgramId);
    Task<BonusProgramReader> GetBonusProgramDetailsByIdAsync(Guid bonusProgramId);
    Task AddNewBonusProgramAsync(BonusProgramReader bonusProgram);
    Task UpdateBonusProgramDepartments(BonusProgramReader bonusProgram);
    Task UpdateBonusProgramAccounts(BonusProgramReader bonusProgram);
}