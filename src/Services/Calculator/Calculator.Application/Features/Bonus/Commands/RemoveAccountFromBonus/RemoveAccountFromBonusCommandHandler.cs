using Calculator.Domain.BonusProgram;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Bonus.Commands.RemoveAccountFromBonus;

public class RemoveAccountFromBonusCommandHandler : ICommandHandler<RemoveAccountFromBonusCommand, Unit>
{
    private readonly IRepository<BonusProgram> _repository;

    public RemoveAccountFromBonusCommandHandler(IRepository<BonusProgram> repository)
    {
        _repository = repository;
    }

    public Task<Unit> Handle(RemoveAccountFromBonusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}