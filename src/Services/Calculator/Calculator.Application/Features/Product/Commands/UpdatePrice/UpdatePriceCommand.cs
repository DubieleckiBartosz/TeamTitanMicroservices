using Calculator.Application.Parameters.ProductParameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Product.Commands.UpdatePrice;

public record UpdatePriceCommand(Guid ProductId, decimal Price) : ICommand<Unit>
{
    public static UpdatePriceCommand Create(UpdatePriceParameters parameters)
    {
        return new UpdatePriceCommand(parameters.ProductId, parameters.Price);
    }
}