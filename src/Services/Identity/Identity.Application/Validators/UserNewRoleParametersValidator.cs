using FluentValidation;
using Identity.Application.Enums;
using Identity.Application.Models.Parameters;

namespace Identity.Application.Validators;

public class UserNewRoleParametersValidator : AbstractValidator<UserNewRoleParameters>
{
    public UserNewRoleParametersValidator()
    {
        RuleFor(r => r.Email).EmailValidator();
        RuleFor(r => r.Role).IsEnumName(typeof(Roles)).Must(_ =>
                !string.Equals(_, Roles.Owner.ToString(), StringComparison.CurrentCultureIgnoreCase))
            .WithMessage("Bad endpoint");
    }
}