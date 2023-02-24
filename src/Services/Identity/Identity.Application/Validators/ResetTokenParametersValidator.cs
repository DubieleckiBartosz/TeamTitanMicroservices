using FluentValidation;
using Identity.Application.Models.Parameters;

namespace Identity.Application.Validators;

public class ResetTokenParametersValidator : AbstractValidator<ResetPasswordParameters>
{
    public ResetTokenParametersValidator()
    {
        RuleFor(r => r.Token).NotEmpty();
        RuleFor(r => r.Password).PasswordValidator();
        RuleFor(r => r.ConfirmPassword).NotEmpty().Equal(x => x.Password)
            .WithMessage("Your passwords are different.");
    }
}