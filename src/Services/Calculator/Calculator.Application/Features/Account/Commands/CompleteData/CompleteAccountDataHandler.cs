using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Account.Commands.CompleteData;

public class CompleteAccountDataHandler : ICommandHandler<CompleteAccountDataCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public CompleteAccountDataHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<Unit> Handle(CompleteAccountDataCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}