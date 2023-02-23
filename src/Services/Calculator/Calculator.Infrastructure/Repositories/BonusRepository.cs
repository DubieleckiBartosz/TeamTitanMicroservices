using Calculator.Application.Contracts;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Calculator.Infrastructure.Repositories;

public class BonusRepository : BaseRepository<BonusRepository>, IBonusRepository
{
    public BonusRepository(string dbConnection, ILoggerManager<BonusRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }
}