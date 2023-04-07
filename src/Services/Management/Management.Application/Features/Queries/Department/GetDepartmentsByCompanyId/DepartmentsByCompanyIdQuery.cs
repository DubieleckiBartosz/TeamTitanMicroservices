using Management.Application.Models.Views;

namespace Management.Application.Features.Queries.Department.GetDepartmentsByCompanyId;

public record DepartmentsByCompanyIdQuery(int CompanyId) : IQuery<List<DepartmentViewModel>>
{
    public static DepartmentsByCompanyIdQuery Create(int companyId)
    {
        return new DepartmentsByCompanyIdQuery(companyId);
    }
}