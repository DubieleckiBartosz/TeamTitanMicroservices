using Calculator.Domain.BonusProgram;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Services;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Bonus.Commands.AddDepartmentToBonus;

public class AddDepartmentToBonusCommandHandler : ICommandHandler<AddDepartmentToBonusCommand, Unit>
{
    private readonly IRepository<BonusProgram> _repository;
    private readonly ICurrentUser _currentUser;

    public AddDepartmentToBonusCommandHandler(IRepository<BonusProgram> repository, ICurrentUser currentUser)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    public async Task<Unit> Handle(AddDepartmentToBonusCommand request, CancellationToken cancellationToken)
    {
        var bonus = await _repository.GetAsync(request.BonusProgram);

        bonus.CheckAndThrowWhenNull("Bonus program");

        var creator = _currentUser.VerificationCode!;
        var department = request.Department;

        bonus.AddDepartmentToBonus(creator, department);

        await _repository.UpdateAsync(bonus);

        return Unit.Value;
    }
}