using Management.Application.Models.DataAccessObjects;
using Management.Domain.Entities;

namespace Management.Application.Contracts.Repositories;

public interface IDepartmentRepository
{
    Task<DepartmentDao?> GetDepartmentByIdAsync(int id);
    Task<List<DepartmentDao>?> GetDepartmentsByCompanyIdAsync(int companyId);
    Task AddNewEmployeeAsync(Department department);
}