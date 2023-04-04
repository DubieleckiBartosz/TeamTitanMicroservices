using Management.Domain.Entities;

namespace Management.Application.Contracts.Repositories;

public interface IEmployeeRepository
{
    Task<Employee> GetEmployeeByCodeAsync(string code);
}