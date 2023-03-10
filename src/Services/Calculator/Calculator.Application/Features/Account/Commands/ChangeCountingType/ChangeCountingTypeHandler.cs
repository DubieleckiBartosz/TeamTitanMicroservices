using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Account.Commands.ChangeCountingType;

public class ChangeCountingTypeHandler : ICommandHandler<ChangeCountingTypeCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public ChangeCountingTypeHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<Unit> Handle(ChangeCountingTypeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}