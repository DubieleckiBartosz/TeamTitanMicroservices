using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.ChangeCountingType;

public class ChangeCountingTypeHandler : ICommandHandler<ChangeCountingTypeCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public ChangeCountingTypeHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Unit> Handle(ChangeCountingTypeCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAsync(request.AccountId);

        account.CheckAndThrowWhenNull("Account");

        account.UpdateCountingType(request.NewCountingType);

        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}