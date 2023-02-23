using Calculator.Application.Contracts;

namespace Calculator.Application.Projections;

public class BonusProjection
{
    private readonly IBonusRepository _bonusRepository;

    public BonusProjection(IBonusRepository bonusRepository)
    {
        _bonusRepository = bonusRepository ?? throw new ArgumentNullException(nameof(bonusRepository));
    }
}