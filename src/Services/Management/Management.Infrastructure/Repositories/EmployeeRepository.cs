using Management.Application.Contracts.Repositories;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Management.Infrastructure.Repositories;

public class EmployeeRepository : BaseRepository<EmployeeRepository>, IEmployeeRepository
{
    public EmployeeRepository(string dbConnection, ILoggerManager<EmployeeRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }
}