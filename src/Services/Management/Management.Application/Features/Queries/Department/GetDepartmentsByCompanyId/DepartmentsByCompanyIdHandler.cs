using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using Management.Application.Models.DataAccessObjects;
using Management.Application.Models.Views;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Mappings;

namespace Management.Application.Features.Queries.Department.GetDepartmentsByCompanyId;

public class DepartmentsByCompanyIdHandler : IQueryHandler<DepartmentsByCompanyIdQuery, List<DepartmentViewModel>>
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentsByCompanyIdHandler(IUnitOfWork unitOfWork)
    {
        _departmentRepository = unitOfWork?.DepartmentRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<List<DepartmentViewModel>> Handle(DepartmentsByCompanyIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _departmentRepository.GetDepartmentsByCompanyIdAsync(request.CompanyId);
        if (result == null)
        {
            throw new NotFoundException(Messages.ListNullOrEmptyMessage("Department"),
                Titles.MethodFailedTitle("GetDepartmentsByCompanyIdAsync"));
        }

        return Mapping.MapList<DepartmentDao, DepartmentViewModel>(result);
    }
}