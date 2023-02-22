using Shared.Domain.Abstractions;

namespace Calculator.Domain.Product.Events;

public record ProductPriceUpdated(decimal NewPrice, Guid ProductId) : IEvent
{
    public static ProductPriceUpdated Create(decimal newPrice, Guid productId)
    {
        return new ProductPriceUpdated(newPrice, productId);
    }
}