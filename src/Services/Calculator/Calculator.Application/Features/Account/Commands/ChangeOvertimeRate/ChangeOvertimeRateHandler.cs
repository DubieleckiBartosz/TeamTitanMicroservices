using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Account.Commands.ChangeOvertimeRate;

public class ChangeOvertimeRateHandler : ICommandHandler<ChangeOvertimeRateCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public ChangeOvertimeRateHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    } 

    public Task<Unit> Handle(ChangeOvertimeRateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}