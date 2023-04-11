using System.Data;
using Dapper;
using Management.Application.Contracts.Repositories;
using Management.Application.Models.DataAccessObjects;
using Management.Domain.Entities;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Management.Infrastructure.Repositories;

public class CompanyRepository : BaseRepository<CompanyRepository>, ICompanyRepository
{
    public CompanyRepository(string dbConnection, ILoggerManager<CompanyRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }

    public async Task<bool> CompanyCodeExistsAsync(string companyCode)
    {
        var param = new DynamicParameters();

        param.Add("@companyCode", companyCode);

        var result = await QueryAsync<bool>("company_codeExists_S", param, CommandType.StoredProcedure);

        return result.FirstOrDefault();
    }

    public async Task<bool> CompanyNameExistsAsync(string companyName)
    {
        var param = new DynamicParameters();

        param.Add("@companyName", companyName);

        var result = await QueryAsync<bool>("company_nameExists_S", param, CommandType.StoredProcedure);

        return result.FirstOrDefault();
    }

    public async Task InitCompanyAsync(Company company, IDbTransaction? transaction)
    {
        var param = new DynamicParameters();
 
        param.Add("@ownerId", company.OwnerId);
        param.Add("@companyCode", company.CompanyCode);
        param.Add("@ownerCode", company.OwnerCode);
        param.Add("@isConfirmed", company.IsConfirmed);
        param.Add("@companyStatus", company.CompanyStatus.Id);

        var result = await ExecuteAsync("company_initCompany_I", param, CommandType.StoredProcedure, transaction);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'company_initCompany_I' failed", "Database Error");
        } 
    }


    public async Task CompleteDataAsync(Company company)
    {
        var param = new DynamicParameters();

        param.Add("@companyId", company.Id);
        param.Add("@companyName", company.CompanyName.ToString());
        param.Add("@isConfirmed", company.IsConfirmed);
        param.Add("@from", company.OpeningHours?.From);
        param.Add("@to", company.OpeningHours?.To);
        param.Add("@city", company.CommunicationData!.Address.City);
        param.Add("@street", company.CommunicationData.Address.Street);
        param.Add("@numberStreet", company.CommunicationData.Address.NumberStreet);
        param.Add("@postalCode", company.CommunicationData.Address.PostalCode);
        param.Add("@phoneNumber", company.CommunicationData.Contact.PhoneNumber);
        param.Add("@email", company.CommunicationData.Contact.Email);
        param.Add("@version", company.Version);

        var result = await ExecuteAsync("company_completeData_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'company_completeData_U' failed", "Database Error"); 
        }
    }

    public async Task AddNewDepartmentAsync(Company company)
    {
        var newDepartment = company.Departments.Last();

        var param = new DynamicParameters();

        param.Add("@companyId", company.Id);
        param.Add("@version", company.Version);
        param.Add("@departmentName", newDepartment.DepartmentName.ToString());

        var result = await ExecuteAsync("department_newDepartment_I", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'department_newDepartment_I' failed", "Database Error");
        }
    }

    public async Task UpdateCommunicationDataAsync(Company company)
    { 
        var param = new DynamicParameters();

        param.Add("@companyId", company.Id);
        param.Add("@city", company.CommunicationData!.Address.City);
        param.Add("@street", company.CommunicationData.Address.Street);
        param.Add("@numberStreet", company.CommunicationData.Address.NumberStreet);
        param.Add("@postalCode", company.CommunicationData.Address.PostalCode);
        param.Add("@phoneNumber", company.CommunicationData.Contact.PhoneNumber);
        param.Add("@email", company.CommunicationData.Contact.Email);
        param.Add("@version", company.CommunicationData.Version);

        var result = await ExecuteAsync("company_updateCommunicationData_U", param, CommandType.StoredProcedure);
        if (result <= 0)
        {
            throw new DatabaseException("The call to procedure 'company_updateCommunicationData_U' failed", "Database Error");
        }
    }

    public async Task<CompanyDao?> GetCompanyByOwnerCodeAsync(string ownerCode)
    {
        var param = new DynamicParameters();

        param.Add("@ownerCode", ownerCode);

        var result = (await QueryAsync<CompanyDao>("company_getByOwnerCode_S", param, CommandType.StoredProcedure)).FirstOrDefault();

        return result;
    }

    public async Task<CompanyDao?> GetCompanyByCodeAsync(string companyCode)
    {
        var param = new DynamicParameters();

        param.Add("@companyCode", companyCode);

        var result = (await QueryAsync<CompanyDao>("company_getByCode_S", param, CommandType.StoredProcedure)).FirstOrDefault();

        return result;
    }

    public async Task<CompanyDao?> GetCompanyWithDepartmentsByCodeAsync(string companyCode)
    {
        var param = new DynamicParameters();

        param.Add("@companyCode", companyCode);

        var dict = new Dictionary<int, CompanyDao>();

        var result =
            (await QueryAsync<CompanyDao, DepartmentDao?, CompanyDao>("company_getWithDepartmentsByCode_S",
                (c, d) =>
                {
                    if (!dict.TryGetValue(c.Id, out var value))
                    {
                        value = c;
                        dict.Add(c.Id, value);
                    }

                    if (d != null)
                    {
                        value.Departments.Add(d);
                    }

                    return value;
                }, splitOn: "Id,Id", param
             , CommandType.StoredProcedure)).FirstOrDefault();

        return result;
    }
}