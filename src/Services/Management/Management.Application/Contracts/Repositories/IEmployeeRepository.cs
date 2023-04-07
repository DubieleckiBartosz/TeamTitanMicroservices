using Management.Domain.Entities;

namespace Management.Application.Contracts.Repositories;

public interface IEmployeeRepository
{
    Task<Employee?> GetEmployeeWithDetailsByIdAsync(int id);
    Task<Employee?> GetEmployeeWithDetailsByCodeAsync(string code);
    Task AddAccountToEmployeeAsync(Employee employee);
    Task AddDayOffRequestToEmployeeAsync(Employee employee);
    Task AddContractToEmployeeAsync(Employee employee);
    Task UpdateContactDataAsync(Employee employee);
    Task UpdateAddressAsync(Employee employee);
}