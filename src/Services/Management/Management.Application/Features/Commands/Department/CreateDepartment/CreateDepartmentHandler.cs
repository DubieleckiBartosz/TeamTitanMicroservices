using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace Management.Application.Features.Commands.Department.CreateDepartment;

public class CreateDepartmentHandler : ICommandHandler<CreateDepartmentCommand, Unit>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICurrentUser _currentUser;

    public CreateDepartmentHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _companyRepository = unitOfWork?.CompanyRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<Unit> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var companyCode = _currentUser.OrganizationCode!;
        var company = (await _companyRepository.GetCompanyWithDepartmentsByCodeAsync(companyCode))?.Map();
        if (company == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Company"),
                Titles.MethodFailedTitle("GetCompanyWithDepartmentsByCodeAsync"));
        }

        company.AddNewDepartment(request.DepartmentName);
        await _companyRepository.AddNewDepartmentAsync(company);

        return Unit.Value;
    }
}