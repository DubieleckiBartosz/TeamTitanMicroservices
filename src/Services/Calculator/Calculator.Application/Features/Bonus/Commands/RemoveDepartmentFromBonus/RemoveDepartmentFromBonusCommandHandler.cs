using Calculator.Domain.BonusProgram;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Bonus.Commands.RemoveDepartmentFromBonus;

public class RemoveDepartmentFromBonusCommandHandler : ICommandHandler<RemoveDepartmentFromBonusCommand, Unit>
{
    private readonly IRepository<BonusProgram> _repository;

    public RemoveDepartmentFromBonusCommandHandler(IRepository<BonusProgram> repository)
    {
        _repository = repository;
    }

    public Task<Unit> Handle(RemoveDepartmentFromBonusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}