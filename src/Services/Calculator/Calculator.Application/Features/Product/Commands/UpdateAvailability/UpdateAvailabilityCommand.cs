using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Product.Commands.UpdateAvailability;

public record UpdateAvailabilityCommand(Guid ProductId) : ICommand<Unit>
{
    public static UpdateAvailabilityCommand Create(Guid productId) => new UpdateAvailabilityCommand(productId);
}