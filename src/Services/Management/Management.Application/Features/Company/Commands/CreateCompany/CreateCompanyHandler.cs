namespace Management.Application.Features.Company.Commands.CreateCompany;

public class CreateCompanyHandler : ICommandHandler<CreateCompanyCommand, int>
{
    public CreateCompanyHandler()
    {
        
    }
    public Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}