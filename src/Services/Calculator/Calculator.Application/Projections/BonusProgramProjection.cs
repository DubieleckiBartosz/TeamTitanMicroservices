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
         
        this.Projects<BonusRecipientAdded>(Handle); 
        this.Projects<BonusRecipientCanceled>(Handle);
        this.Projects<NewBonusProgramCreated>(Handle);
    }
     
    public async Task Handle(BonusRecipientAdded @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var bonusProgram = await _bonusProgramRepository.GetBonusProgramDetailsByIdAsync(@event.BonusProgramId);
        this.CheckBonusProgram(bonusProgram);

        bonusProgram.RecipientToBonusProgramAdded(@event);
        await _bonusProgramRepository.AddBonusRecipientAsync(bonusProgram);
    } 

    public async Task Handle(BonusRecipientCanceled @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var bonusProgram = await _bonusProgramRepository.GetBonusProgramByIdAsync(@event.BonusProgramId);
        this.CheckBonusProgram(bonusProgram);

        bonusProgram.RecipientFromBonusProgramCanceled(@event);
        await _bonusProgramRepository.UpdateBonusRecipientAsync(bonusProgram);
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