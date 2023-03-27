using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Services;

namespace Management.Application.Features.Company.Commands.CompleteData;

public class CompleteDataHandler : ICommandHandler<CompleteDataCommand, Unit>
{
    private readonly ICurrentUser _currentUser;
    private readonly ICompanyRepository _companyRepository;

    public CompleteDataHandler(ICurrentUser currentUser, ICompanyRepository companyRepository)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
    }

    public async Task<Unit> Handle(CompleteDataCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}