using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.AddPieceProduct;

public class AddPieceProductHandler : ICommandHandler<AddPieceProductCommand, Unit>
{
    public Task<Unit> Handle(AddPieceProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}