using Management.Application.Contracts.Repositories;
using Management.Domain.Entities;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Management.Infrastructure.Repositories;

public class CompanyRepository : BaseRepository<CompanyRepository>, ICompanyRepository
{
    public CompanyRepository(string dbConnection, ILoggerManager<CompanyRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }

    public Task<bool> CompanyCodeExistsAsync(string companyCode)
    {
        throw new NotImplementedException();
    }

    public Task InitCompanyAsync(Company company)
    {
        throw new NotImplementedException();
    }

    public Task CompleteDataAsync(Company company)
    {
        throw new NotImplementedException();
    }

    public Task<Company> GetCompanyByCodeAsync(string companyCode)
    {
        throw new NotImplementedException();
    }

    public Task<Company> GetCompanyWithDepartmentsByCodeAsync(string companyCode)
    {
        throw new NotImplementedException();
    }
}