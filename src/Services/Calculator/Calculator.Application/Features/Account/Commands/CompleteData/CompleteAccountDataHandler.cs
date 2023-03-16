using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.CompleteData;

public class CompleteAccountDataHandler : ICommandHandler<CompleteAccountDataCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public CompleteAccountDataHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Unit> Handle(CompleteAccountDataCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAsync(request.AccountId);

        account.CheckAndThrowWhenNull("Recipient");

        var countingType = request.CountingType;
        var workDayHours = request.WorkDayHours;
        var overtimeRate = request.OvertimeRate;
        var hourlyRate = request.HourlyRate;
        var expirationDate = request.ExpirationDate;

        account.CompleteAccount(countingType, workDayHours, overtimeRate, hourlyRate, expirationDate);

        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}