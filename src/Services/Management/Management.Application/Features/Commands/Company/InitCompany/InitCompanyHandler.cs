using Management.Application.Contracts.Repositories;
using Management.Application.Generators;
using MediatR;
using Shared.Implementations.Dapper;
using Shared.Implementations.Services;

namespace Management.Application.Features.Commands.Company.InitCompany;

[WithTransaction]
public class InitCompanyHandler : ICommandHandler<InitCompanyCommand, Unit>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransaction _transaction;
    private readonly ICompanyRepository _companyRepository;

    public InitCompanyHandler(ICurrentUser currentUser, IUnitOfWork unitOfWork, ITransaction transaction)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        _companyRepository = unitOfWork.CompanyRepository;
    }

    public async Task<Unit> Handle(InitCompanyCommand request, CancellationToken cancellationToken)
    { 
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

        var ownerCode = CodeGenerators.PersonCompanyCodeGenerate(companyCode);
        var newCompany = Domain.Entities.Company.Init(_currentUser.UserId, companyCode, ownerCode);

        await _companyRepository.InitCompanyAsync(newCompany, _transaction.GetTransactionWhenExist());
        await _unitOfWork.CompleteAsync(newCompany);

        return Unit.Value;
    }
}