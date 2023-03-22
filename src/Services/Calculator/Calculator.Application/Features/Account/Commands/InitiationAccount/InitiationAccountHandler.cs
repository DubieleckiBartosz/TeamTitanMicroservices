using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Account.Commands.InitiationAccount;

public class InitiationAccountHandler : ICommandHandler<InitiationAccountCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public InitiationAccountHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<Unit> Handle(InitiationAccountCommand request, CancellationToken cancellationToken)
    {
        var companyCode = request.CompanyCode;
        var accountCode = request.AccountOwnerCode;
        var creator = request.Creator;

        var newAccount = Domain.Account.Account.Create(companyCode, accountCode, creator);
        await _repository.AddAsync(newAccount);

        return Unit.Value;
    }
}