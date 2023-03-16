using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.CancelBonus;

public class CancelBonusAccountCommandHandler : ICommandHandler<CancelBonusAccountCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public CancelBonusAccountCommandHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(CancelBonusAccountCommand request, CancellationToken cancellationToken)
    {
        var bonus = await _repository.GetAsync(request.AccountId);

        bonus.CheckAndThrowWhenNull("Account");

        bonus.CancelBonus(request.BonusCode);
        await _repository.UpdateAsync(bonus);

        return Unit.Value;
    }
}