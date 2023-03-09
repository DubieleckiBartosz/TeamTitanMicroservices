using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Management.Infrastructure.Repositories;

public class CompanyRepository : BaseRepository<CompanyRepository>
{
    public CompanyRepository(string dbConnection, ILoggerManager<CompanyRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }
}