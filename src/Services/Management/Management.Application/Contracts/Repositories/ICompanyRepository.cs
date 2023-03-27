using Management.Domain.Entities;

namespace Management.Application.Contracts.Repositories;

public interface ICompanyRepository
{
    Task<bool> CompanyCodeExistsAsync(string companyCode);
    Task InitCompanyAsync(Company company);
    Task CompleteDataAsync(Company company);
    Task<Company> GetCompanyByCodeAsync(string companyCode); 
    Task<Company> GetCompanyWithDepartmentsByCodeAsync(string companyCode); 
}