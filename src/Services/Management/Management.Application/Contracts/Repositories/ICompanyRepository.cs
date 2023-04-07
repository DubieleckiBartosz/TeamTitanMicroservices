using Management.Domain.Entities;
using System.Data;

namespace Management.Application.Contracts.Repositories;

public interface ICompanyRepository
{
    Task<bool> CompanyCodeExistsAsync(string companyCode);
    Task<bool> CompanyNameExistsAsync(string companyName);
    Task InitCompanyAsync(Company company, IDbTransaction? transaction);
    Task CompleteDataAsync(Company company);
    Task AddNewDepartmentAsync(Company company);
    Task UpdateCommunicationDataAsync(Company company);
    Task<Company?> GetCompanyByOwnerCodeAsync(string ownerCode);
    Task<Company?> GetCompanyByCodeAsync(string companyCode); 
    Task<Company?> GetCompanyWithDepartmentsByCodeAsync(string companyCode); 
}