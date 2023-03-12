using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.ChangeDayHours;

public class ChangeDayHoursHandler : ICommandHandler<ChangeDayHoursCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public ChangeDayHoursHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Unit> Handle(ChangeDayHoursCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAsync(request.AccountId);

        account.CheckAndThrowWhenNull("Recipient");

        account.UpdateWorkDayHours(request.NewWorkDayHours);

        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}