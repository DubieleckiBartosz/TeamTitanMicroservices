using Shared.Domain.Abstractions;

namespace Calculator.Domain.Account.Events;

public record PieceProductAdded(Guid PieceworkProductId, decimal Quantity, decimal CurrentPrice, Guid AccountId,
    DateTime Date) : IEvent
{
    public static PieceProductAdded Create(Guid pieceworkProductId, decimal quantity, decimal currentPrice,
        Guid accountId, DateTime date)
    {
        return new PieceProductAdded(pieceworkProductId, quantity, currentPrice, accountId, date);
    }
}