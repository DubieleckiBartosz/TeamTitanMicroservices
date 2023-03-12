using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.ChangeHourlyRate;

public class ChangeHourlyRateHandler : ICommandHandler<ChangeHourlyRateCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public ChangeHourlyRateHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Unit> Handle(ChangeHourlyRateCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAsync(request.AccountId);

        account.CheckAndThrowWhenNull("Recipient");

        account.UpdateHourlyRate(request.NewHourlyRate);

        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}