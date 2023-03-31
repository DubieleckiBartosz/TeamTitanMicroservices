using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using Management.Application.Generators;
using MediatR;
using Shared.Domain.DomainExceptions;
using Shared.Implementations.Dapper;
using Shared.Implementations.Services;

namespace Management.Application.Features.Company.Commands.InitCompany;

[WithTransaction]
public class InitCompanyHandler : ICommandHandler<InitCompanyCommand, Unit>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;

    public InitCompanyHandler(ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _companyRepository = unitOfWork.CompanyRepository;
    }

    public async Task<Unit> Handle(InitCompanyCommand request, CancellationToken cancellationToken)
    {
        if (this._currentUser.IsInRole(Positions.OwnerPosition))
        {
            throw new BusinessException("Incorrect role", "The current user is already an owner.");
        }

        var companyCode = string.Empty;
        var repeat = true;
        while (repeat)
        {
            companyCode = CodeGenerators.CompanyCodeGenerate();
            var result = await _companyRepository.CompanyCodeExistsAsync(companyCode);
            if (!result)
            {
                repeat = false;
            }
        }

        var ownerCode = CodeGenerators.PersonCompanyCodeGenerate(companyCode, Positions.OwnerPosition);
        var newCompany = Domain.Entities.Company.Init(_currentUser.UserId, companyCode, ownerCode);

        await _companyRepository.InitCompanyAsync(newCompany);
        await _unitOfWork.CompleteAsync(newCompany);

        return Unit.Value;
    }
}