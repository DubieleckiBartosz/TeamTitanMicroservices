using Calculator.Application.Parameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.AddPieceProduct;

public record AddPieceProductCommand(Guid PieceworkProductId, decimal Quantity, decimal CurrentPrice, Guid AccountId,
    DateTime? Date) : ICommand<Unit>
{
    public static AddPieceProductCommand Create(AddPieceProductParameters parameters)
    {
        return new AddPieceProductCommand()
    }
}