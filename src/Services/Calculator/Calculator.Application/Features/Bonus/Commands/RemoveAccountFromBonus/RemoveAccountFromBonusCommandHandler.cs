using Calculator.Domain.BonusProgram;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Bonus.Commands.RemoveAccountFromBonus;

public class RemoveAccountFromBonusCommandHandler : ICommandHandler<RemoveAccountFromBonusCommand, Unit>
{
    private readonly IRepository<BonusProgram> _repository;

    public RemoveAccountFromBonusCommandHandler(IRepository<BonusProgram> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(RemoveAccountFromBonusCommand request, CancellationToken cancellationToken)
    {
        var bonus = await _repository.GetAsync(request.BonusProgram);

        bonus.CheckAndThrowWhenNull("Bonus program");

        var account = request.Account;

        bonus.RemoveAccountFromBonus(account);

        await _repository.UpdateAsync(bonus);

        return Unit.Value;
    }
}