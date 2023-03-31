using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Services;

namespace Management.Application.Features.Company.Commands.CompleteData;

public class CompleteDataHandler : ICommandHandler<CompleteDataCommand, Unit>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;

    public CompleteDataHandler(ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _companyRepository = unitOfWork.CompanyRepository;
    }

    public async Task<Unit> Handle(CompleteDataCommand request, CancellationToken cancellationToken)
    {
        var company = _companyRepository.GetCompanyByOwnerCodeAsync(_currentUser.VerificationCode!);

        return Unit.Value;
    }
}