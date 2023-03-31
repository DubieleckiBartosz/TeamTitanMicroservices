using Shared.Domain.Base;

namespace Management.Application.Contracts.Repositories;

public interface IUnitOfWork
{
    ICompanyRepository CompanyRepository { get; }
    IDepartmentRepository DepartmentRepository { get; }
    IEmployeeRepository EmployeeRepository { get; }
    Task CompleteAsync<T>(T entity) where T : Entity;
}