using Calculator.Application.Features.Product.Commands.UpdateAvailability;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.ProductCommandValidators;

public class UpdateAvailabilityCommandValidator : AbstractValidatorNotNull<UpdateAvailabilityCommand>
{
    public UpdateAvailabilityCommandValidator()
    {
        RuleFor(_ => _.ProductId).SetValidator(new GuidValidator());

    }
}