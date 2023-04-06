using Management.Domain.Entities;

namespace Management.Application.Contracts.Repositories;

public interface IDepartmentRepository
{
    Task<Department?> GetDepartmentByIdAsync(int id);
    Task AddNewEmployeeAsync(Department department);
}