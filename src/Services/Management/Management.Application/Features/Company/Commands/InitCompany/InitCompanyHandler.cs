using Management.Application.Contracts.Repositories;
using Shared.Implementations.Services;

namespace Management.Application.Features.Company.Commands.InitCompany;

public class InitCompanyHandler : ICommandHandler<InitCompanyCommand, int>
{
    private readonly ICurrentUser _currentUser;
    private readonly ICompanyRepository _companyRepository;

    public InitCompanyHandler(ICurrentUser currentUser, ICompanyRepository companyRepository)
    {
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
    }

    public Task<int> Handle(InitCompanyCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}