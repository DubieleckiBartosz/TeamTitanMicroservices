using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record PieceProductAdded(Guid PieceworkProductId, decimal Quantity, decimal CurrentPrice, Guid AccountId) : IEvent
{
    public static PieceProductAdded Create(Guid pieceworkProductId, decimal quantity, decimal currentPrice, Guid accountId)
    {
        return new PieceProductAdded(pieceworkProductId, quantity, currentPrice, accountId);
    }
}