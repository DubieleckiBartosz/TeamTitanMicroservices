namespace Calculator.Application.Features.Account.Commands.AddPieceProduct;

public record AddPieceProductCommand(Guid PieceworkProductId, decimal Quantity, decimal CurrentPrice, Guid AccountId)
{
}