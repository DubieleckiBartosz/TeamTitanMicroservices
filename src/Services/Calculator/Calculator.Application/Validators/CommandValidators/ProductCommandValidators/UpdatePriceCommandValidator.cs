using Calculator.Application.Features.Product.Commands.UpdatePrice;
using FluentValidation;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.ProductCommandValidators;

public class UpdatePriceCommandValidator : AbstractValidatorNotNull<UpdatePriceCommand>
{
    public UpdatePriceCommandValidator()
    {
        RuleFor(_ => _.ProductId).SetValidator(new GuidValidator());
        RuleFor(_ => _.Price).GreaterThan(0);
    }
}