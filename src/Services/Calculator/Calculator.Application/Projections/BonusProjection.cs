using Calculator.Application.Contracts;

namespace Calculator.Application.Projections;

public class BonusProjection
{
    private readonly IWrapperRepository _wrapperRepository;

    public BonusProjection(IWrapperRepository wrapperRepository)
    {
        _wrapperRepository = wrapperRepository ?? throw new ArgumentNullException(nameof(wrapperRepository));
    }
}