using Management.Domain.Entities;

namespace Management.Application.Contracts.Repositories;

public interface IEmployeeRepository
{
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<Employee?> GetEmployeeByCodeAsync(string code);
    Task AddAccountToEmployeeAsync(Employee employee);
    Task UpdateContactDataAsync(Employee employee);
    Task UpdateAddressAsync(Employee employee);
}