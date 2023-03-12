using Calculator.Domain.BonusProgram;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Services;

namespace Calculator.Application.Features.Bonus.Commands.AddBonusProgram;

public class AddBonusProgramCommandHandler : ICommandHandler<AddBonusProgramCommand, Guid>
{
    private readonly IRepository<BonusProgram> _repository;
    private readonly ICurrentUser _currentUser;

    public AddBonusProgramCommandHandler(IRepository<BonusProgram> repository, ICurrentUser currentUser)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    public async Task<Guid> Handle(AddBonusProgramCommand request, CancellationToken cancellationToken)
    {
        var bonusAmount = request.BonusAmount;
        var createdBy = _currentUser.VerificationCode!;
        var companyCode = request.CompanyCode; 
        var expires = request.Expires;
        var reason = request.Reason;

        var newBonus = BonusProgram.Create(bonusAmount, createdBy, companyCode, expires, reason);

        await _repository.AddAsync(newBonus);

        return newBonus.Id;
    }
}