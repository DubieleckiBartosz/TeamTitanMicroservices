using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.ChangeOvertimeRate;

public class ChangeOvertimeRateHandler : ICommandHandler<ChangeOvertimeRateCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public ChangeOvertimeRateHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    } 

    public async Task<Unit> Handle(ChangeOvertimeRateCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAsync(request.AccountId);

        account.CheckAndThrowWhenNull("Recipient");

        account.UpdateOvertimeRate(request.NewOvertimeRate);

        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}