using Shared.Domain.Abstractions;

namespace Calculator.Domain.Product.Events;

public record AvailabilityUpdated(bool IsAvailable, Guid ProductId) : IEvent
{
    public static AvailabilityUpdated Create(bool isAvailable, Guid productId)
    {
        return new AvailabilityUpdated(isAvailable, productId);
    }
}