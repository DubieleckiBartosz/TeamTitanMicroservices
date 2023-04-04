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

    public Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Employee?> GetEmployeeByCodeAsync(string code)
    {
        throw new NotImplementedException();
    }

    public Task AddAccountToEmployeeAsync(Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task UpdateContactDataAsync(Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAddressAsync(Employee employee)
    {
        throw new NotImplementedException();
    }
}