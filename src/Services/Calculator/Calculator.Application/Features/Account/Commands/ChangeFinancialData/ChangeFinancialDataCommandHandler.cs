using Calculator.Domain.Account.Snapshots;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.ChangeFinancialData;

public class ChangeFinancialDataCommandHandler : ICommandHandler<ChangeFinancialDataCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public ChangeFinancialDataCommandHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    } 

    public async Task<Unit> Handle(ChangeFinancialDataCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAggregateFromSnapshotAsync<AccountSnapshot>(request.AccountId);

        account.CheckAndThrowWhenNullOrNotMatch("Account");

        var overtime = request.OvertimeRate;
        var hourlyRate = request.HourlyRate;

        account!.AccountUpdateFinancialData(overtime, hourlyRate);

        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}