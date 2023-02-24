using FluentValidation;
using Identity.Application.Models.Parameters;

namespace Identity.Application.Validators;

public class ForgotPasswordParametersValidator : AbstractValidator<ForgotPasswordParameters>
{
    public ForgotPasswordParametersValidator()
    {
        this.RuleFor(r => r.Email).EmailValidator();
    }
}