using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Account.Commands.AddPieceProduct;

public class AddPieceProductHandler : ICommandHandler<AddPieceProductCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public AddPieceProductHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<Unit> Handle(AddPieceProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}