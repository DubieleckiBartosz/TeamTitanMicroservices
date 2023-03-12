using Calculator.Domain.BonusProgram;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Bonus.Commands.CancelDepartmentBonus;

public class CancelBonusRecipientCommandHandler : ICommandHandler<CancelBonusRecipientCommand, Unit>
{
    private readonly IRepository<BonusProgram> _repository;

    public CancelBonusRecipientCommandHandler(IRepository<BonusProgram> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(CancelBonusRecipientCommand request, CancellationToken cancellationToken)
    {
        var bonus = await _repository.GetAsync(request.BonusProgram);

        bonus.CheckAndThrowWhenNull("Bonus program");

        bonus.CancelBonusRecipient(request.RecipientCode, request.BonusCode);
        await _repository.UpdateAsync(bonus);

        return Unit.Value;
    }
}