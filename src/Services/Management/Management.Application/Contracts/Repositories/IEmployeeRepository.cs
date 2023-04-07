using Management.Application.Models.DataAccessObjects;
using Management.Domain.Entities;

namespace Management.Application.Contracts.Repositories;

public interface IEmployeeRepository
{
    Task<EmployeeDao?> GetEmployeeWithDetailsByIdAsync(int id);
    Task<EmployeeDao?> GetEmployeeWithDetailsByCodeAsync(string code);
    Task AddAccountToEmployeeAsync(Employee employee);
    Task AddDayOffRequestToEmployeeAsync(Employee employee);
    Task AddContractToEmployeeAsync(Employee employee);
    Task UpdateContactDataAsync(Employee employee);
    Task UpdateAddressAsync(Employee employee);
}