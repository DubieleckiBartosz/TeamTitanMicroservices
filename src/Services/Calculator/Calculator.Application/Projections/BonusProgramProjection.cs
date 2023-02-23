using Calculator.Application.Contracts;
using Calculator.Application.ReadModels.BonusReaders;
using Calculator.Domain.BonusProgram.Events;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Projection;

namespace Calculator.Application.Projections;

public class BonusProgramProjection : ReadModelAction<BonusProgramProjection>
{
    private readonly IBonusProgramRepository _bonusProgramRepository;

    public BonusProgramProjection(IBonusProgramRepository bonusProgramRepository)
    {
        _bonusProgramRepository = bonusProgramRepository ?? throw new ArgumentNullException(nameof(bonusProgramRepository));

        this.Projects<AccountRemoved>(Handle);
        this.Projects<BonusAccountAdded>(Handle);
        this.Projects<BonusDepartmentAdded>(Handle);
        this.Projects<DepartmentRemoved>(Handle);
        this.Projects<NewBonusProgramCreated>(Handle);
    }

    public async Task Handle(AccountRemoved @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var bonusProgram = await _bonusProgramRepository.GetBonusProgramWithAccountsByIdAsync(@event.BonusProgramId);
        this.CheckBonusProgram(bonusProgram);

        bonusProgram.AccountFromBonusRemoved(@event);
        await _bonusProgramRepository.UpdateBonusProgramAccounts(bonusProgram);
    }
    public async Task Handle(BonusAccountAdded @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var bonusProgram = await _bonusProgramRepository.GetBonusProgramWithAccountsByIdAsync(@event.BonusProgramId);
        this.CheckBonusProgram(bonusProgram);

        bonusProgram.AccountToBonusAdded(@event);
        await _bonusProgramRepository.UpdateBonusProgramAccounts(bonusProgram);
    }
    public async Task Handle(BonusDepartmentAdded @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var bonusProgram = await _bonusProgramRepository.GetBonusProgramWithDepartmentsByIdAsync(@event.BonusProgramId);
        this.CheckBonusProgram(bonusProgram);

        bonusProgram.DepartmentToBonusAdded(@event);
        await _bonusProgramRepository.UpdateBonusProgramDepartments(bonusProgram);
    }
    public async Task Handle(DepartmentRemoved @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var bonusProgram = await _bonusProgramRepository.GetBonusProgramWithDepartmentsByIdAsync(@event.BonusProgramId);
        this.CheckBonusProgram(bonusProgram);

        bonusProgram.DepartmentFromBonusRemoved(@event);
        await _bonusProgramRepository.UpdateBonusProgramDepartments(bonusProgram);
    }
    public async Task Handle(NewBonusProgramCreated @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var newBonusProgram = BonusProgramReader.BonusCreate(@event);
        await _bonusProgramRepository.AddNewBonusProgramAsync(newBonusProgram);
    }

    private void CheckBonusProgram(BonusProgramReader bonusProgram)
    {
        if (bonusProgram == null)
        {
            throw new NotFoundException("Program cannot be null.", "Bonus program not found");
        }
    }
}