using Management.Application.Models.DataAccessObjects;
using Management.Domain.Entities;

namespace Management.Application.Contracts.Repositories;

public interface IEmployeeRepository
{
    Task<EmployeeDao?> GetEmployeeNecessaryDataByIdAsync(int id);
    Task<EmployeeDao?> GetEmployeeWithContractsByIdAsync(int id);
    Task<EmployeeDao?> GetEmployeeWithCommunicationDataByIdAsync(int id);
    Task<EmployeeDao?> GetEmployeeWithDetailsByIdAsync(int id); 
    Task<EmployeeDao?> GetEmployeeWithDetailsByCodeAsync(string code);
    Task<EmployeeDao?> GetEmployeeNecessaryDataByCodeAsync(string code);
    Task AddAccountToEmployeeAsync(Employee employee);
    Task AddDayOffRequestToEmployeeAsync(Employee employee);
    Task AddContractToEmployeeAsync(Employee employee);
    Task UpdateContactDataAsync(Employee employee);
    Task UpdateAddressAsync(Employee employee);
    Task UpdateLeaderAsync(Employee employee);
}