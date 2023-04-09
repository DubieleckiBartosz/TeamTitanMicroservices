using Calculator.Application.Constants;
using Calculator.Application.Features.Account.Commands.AccountSettlement;
using Calculator.Domain.Account.Snapshots;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Background;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.UpdateSettlementDay;

public class UpdateSettlementDayCommandHandler : ICommandHandler<UpdateSettlementDayCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;
    private readonly IJobService _jobService; 

    public UpdateSettlementDayCommandHandler(IRepository<Domain.Account.Account> repository, IJobService jobService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService)); 
    }

    public async Task<Unit> Handle(UpdateSettlementDayCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAggregateFromSnapshotAsync<AccountSnapshot>(request.Account);

        account.CheckAndThrowWhenNullOrNotMatch("Account");
        
        account!.UpdateSettlementDay(request.NewSettlementDay); 

        var cronExpression = $"0 0 {request.NewSettlementDay} * *";
        _jobService.RecurringJobMediator(Keys.SettlementBackgroundJobName(account!.Id.ToString()), new AccountSettlementCommand(account.Id),
            cronExpression);

        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}