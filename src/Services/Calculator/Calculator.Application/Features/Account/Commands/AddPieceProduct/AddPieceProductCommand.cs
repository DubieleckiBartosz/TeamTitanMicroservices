using Calculator.Application.Parameters.AccountParameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.AddPieceProduct;

public record AddPieceProductCommand(Guid PieceworkProductId, decimal Quantity, decimal CurrentPrice, Guid AccountId,
    DateTime? Date) : ICommand<Unit>
{
    public static AddPieceProductCommand Create(AddPieceProductParameters parameters)
    {
        return new AddPieceProductCommand(parameters.PieceworkProductId, parameters.Quantity, parameters.CurrentPrice,
            parameters.AccountId, parameters.Date);
    }
}