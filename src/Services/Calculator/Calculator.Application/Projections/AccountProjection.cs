using Calculator.Application.Contracts;
using Calculator.Application.ReadModels.AccountReaders;
using Shared.Implementations.Projection;

namespace Calculator.Application.Projections;

public class AccountProjection : ReadModelAction<AccountReader>
{
    private readonly IWrapperRepository _wrapperRepository;

    public AccountProjection(IWrapperRepository wrapperRepository)
    {
        _wrapperRepository = wrapperRepository ?? throw new ArgumentNullException(nameof(wrapperRepository));
    }
}