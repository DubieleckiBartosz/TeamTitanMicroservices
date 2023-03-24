using FluentValidation;

namespace Shared.Implementations.Validators;

public class GuidValidator : AbstractValidator<Guid>
{
    public GuidValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .NotNull()
            .Must(x => x != Guid.Empty)
            .WithMessage("GUID cannot be empty.");
    }
}