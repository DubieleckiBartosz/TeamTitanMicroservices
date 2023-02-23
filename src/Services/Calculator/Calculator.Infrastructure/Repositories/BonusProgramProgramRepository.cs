﻿using Calculator.Application.Contracts;
using Calculator.Application.ReadModels.BonusReaders;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Calculator.Infrastructure.Repositories;

public class BonusProgramProgramRepository : BaseRepository<BonusProgramProgramRepository>, IBonusProgramRepository
{
    public BonusProgramProgramRepository(string dbConnection, ILoggerManager<BonusProgramProgramRepository> loggerManager) : base(dbConnection, loggerManager)
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

    public Task AddNewBonusProgramAsync(BonusProgramReader bonusProgram)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBonusProgramDepartments(BonusProgramReader bonusProgram)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBonusProgramAccounts(BonusProgramReader bonusProgram)
    {
        throw new NotImplementedException();
    }
}