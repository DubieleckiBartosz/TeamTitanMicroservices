using Calculator.Application.Parameters.ProductParameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Product.Commands.CreateNewProduct;

public record CreateNewProductCommand
    (decimal PricePerUnit, string CountedInUnit, string ProductName) : ICommand<Unit>
{
    public static CreateNewProductCommand Create(CreateNewProductParameters parameters)
    {
        return new CreateNewProductCommand(parameters.PricePerUnit, parameters.CountedInUnit, parameters.ProductName);
    }
}