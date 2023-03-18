using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Services;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.AddWorkDay;

public class AddWorkDayHandler : ICommandHandler<AddWorkDayCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;
    private readonly ICurrentUser _currentUser;

    public AddWorkDayHandler(IRepository<Domain.Account.Account> repository, ICurrentUser currentUser)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(AddWorkDayCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAsync(request.AccountId);

        account.CheckAndThrowWhenNull("Account");

        var date = request.Date;
        var hoursWorked = request.HoursWorked;
        var overtime = request.Overtime;
        var isDayOff = request.IsDayOff;
        var createdBy = _currentUser.VerificationCode!;

        account.AddNewWorkDay(date, hoursWorked, overtime, isDayOff, createdBy);

        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}