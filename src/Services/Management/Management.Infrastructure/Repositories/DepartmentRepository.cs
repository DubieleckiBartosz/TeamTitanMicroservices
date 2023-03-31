using Management.Application.Contracts.Repositories;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Management.Infrastructure.Repositories;

public class DepartmentRepository : BaseRepository<DepartmentRepository>, IDepartmentRepository 
{
    public DepartmentRepository(string dbConnection, ILoggerManager<DepartmentRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }
}