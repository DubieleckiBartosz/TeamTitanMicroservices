using Shared.Implementations.Core.Exceptions;

namespace Shared.Implementations.Validators;

public static class AggregateValidator
{
    public static void CheckAndThrowWhenNullOrNotMatch<TAggregate>(this TAggregate? aggregate, string? name = null, Func<TAggregate, bool>? funcValidator = null)
    {
        if (aggregate == null || (funcValidator != null && !funcValidator.Invoke(aggregate)))
        {
            throw new NotFoundException($"{name ?? "Aggregate"} cannot be null", $"{name ?? "Aggregate"} not found");
        } 
    }
}