using Management.Application.Contracts.Repositories;
using Management.Domain.Entities;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Management.Infrastructure.Repositories;

public class DepartmentRepository : BaseRepository<DepartmentRepository>, IDepartmentRepository 
{
    public DepartmentRepository(string dbConnection, ILoggerManager<DepartmentRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }

    public Task<Department?> GetDepartmentByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}