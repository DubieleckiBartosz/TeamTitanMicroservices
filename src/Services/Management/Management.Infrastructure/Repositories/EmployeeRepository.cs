using Management.Application.Contracts.Repositories;
using Management.Domain.Entities;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Management.Infrastructure.Repositories;

public class EmployeeRepository : BaseRepository<EmployeeRepository>, IEmployeeRepository
{
    public EmployeeRepository(string dbConnection, ILoggerManager<EmployeeRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }

    public Task<Employee> GetEmployeeByCodeAsync(string code)
    {
        throw new NotImplementedException();
    }
}