using Management.Domain.Entities;
using System.Data;
using Management.Application.Models.DataAccessObjects;

namespace Management.Application.Contracts.Repositories;

public interface ICompanyRepository
{
    Task<bool> CompanyCodeExistsAsync(string companyCode);
    Task<bool> CompanyNameExistsAsync(string companyName);
    Task InitCompanyAsync(Company company, IDbTransaction? transaction);
    Task CompleteDataAsync(Company company);
    Task AddNewDepartmentAsync(Company company);
    Task UpdateCommunicationDataAsync(Company company);
    Task<CompanyDao?> GetCompanyByOwnerCodeAsync(string ownerCode);
    Task<CompanyDao?> GetCompanyByCodeAsync(string companyCode); 
    Task<CompanyDao?> GetCompanyWithDepartmentsByCodeAsync(string companyCode); 
}