using Shared.Implementations.Core.Exceptions;

namespace Shared.Implementations.Validators;

public static class AggregateValidator
{
    public static void CheckAndThrowWhenNull<TAggregate>(this TAggregate? aggregate, string? name = null)
    {
        if (aggregate == null)
        {
            throw new NotFoundException($"{name ?? "Aggregate"} cannot be null", $"{name ?? "Aggregate"} not found");
        } 
    }
}