using Calculator.Domain.BonusProgram;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Bonus.Commands.RemoveDepartmentFromBonus;

public class RemoveDepartmentFromBonusCommandHandler : ICommandHandler<RemoveDepartmentFromBonusCommand, Unit>
{
    private readonly IRepository<BonusProgram> _repository;

    public RemoveDepartmentFromBonusCommandHandler(IRepository<BonusProgram> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(RemoveDepartmentFromBonusCommand request, CancellationToken cancellationToken)
    {
        var bonus = await _repository.GetAsync(request.BonusProgram);

        bonus.CheckAndThrowWhenNull("Bonus program");

        bonus.RemoveDepartmentFromBonus(request.DepartmentCode);
        await _repository.UpdateAsync(bonus);

        return Unit.Value;
    }
}