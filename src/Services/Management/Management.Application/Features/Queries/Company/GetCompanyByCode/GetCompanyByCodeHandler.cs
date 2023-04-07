using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using Management.Application.Models.DataAccessObjects;
using Management.Application.Models.Views;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Mappings;
using Shared.Implementations.Services;

namespace Management.Application.Features.Queries.Company.GetCompanyByCode;

public class GetCompanyByCodeHandler : IQueryHandler<GetCompanyByCodeQuery, CompanyViewModel>
{
    private readonly ICurrentUser _currentUser;
    private readonly ICompanyRepository _companyRepository;

    public GetCompanyByCodeHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _companyRepository = unitOfWork?.CompanyRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<CompanyViewModel> Handle(GetCompanyByCodeQuery request, CancellationToken cancellationToken)
    {
        var organizationCode = _currentUser.OrganizationCode;
        var result = await _companyRepository.GetCompanyByCodeAsync(organizationCode!);

        if (result == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Company"),
                Titles.MethodFailedTitle("GetCompanyWithDepartmentsByCodeAsync"));
        }

        return Mapping.Map<CompanyDao, CompanyViewModel>(result);
    }
}