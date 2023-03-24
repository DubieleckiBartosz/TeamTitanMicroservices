using Calculator.Application.Features.Product.Commands.CreateNewProduct;
using FluentValidation;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.ProductCommandValidators;

public class CreateNewProductCommandValidator : AbstractValidatorNotNull<CreateNewProductCommand>
{
    public CreateNewProductCommandValidator()
    { 
        RuleFor(_ => _.PricePerUnit).GreaterThan(0);
        RuleFor(_ => _.CountedInUnit).StringValidator();
        RuleFor(_ => _.ProductName).StringValidator();
    }
}