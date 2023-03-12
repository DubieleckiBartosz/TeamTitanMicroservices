using Calculator.Domain.BonusProgram;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Services;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Bonus.Commands.AddAccountToBonus;

public class AddAccountToBonusCommandHandler : ICommandHandler<AddAccountToBonusCommand, Unit>
{
    private readonly IRepository<BonusProgram> _repository;
    private readonly ICurrentUser _currentUser;

    public AddAccountToBonusCommandHandler(IRepository<BonusProgram> repository, ICurrentUser currentUser)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<Unit> Handle(AddAccountToBonusCommand request, CancellationToken cancellationToken)
    {
        var bonus = await _repository.GetAsync(request.BonusProgram);

        bonus.CheckAndThrowWhenNull("Bonus program");

        var creator = _currentUser.VerificationCode!;
        var account = request.Account;

        bonus.AddAccountToBonus(creator, account);

        await _repository.UpdateAsync(bonus);

        return Unit.Value;
    }
}