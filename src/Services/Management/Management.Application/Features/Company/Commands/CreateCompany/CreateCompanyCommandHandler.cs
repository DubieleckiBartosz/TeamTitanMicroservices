namespace Management.Application.Features.Company.Commands.CreateCompany;

public class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, int>
{
    public CreateCompanyCommandHandler()
    {
        
    }
    public Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}