using FluentValidation;

namespace Identity.Application.Validators;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> PasswordValidator<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        //https://stackoverflow.com/questions/48635152/regex-for-default-asp-net-core-identity-password
        return ruleBuilder.NotEmpty()
            .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{8,}$");
    }

    public static IRuleBuilderOptions<T, string> EmailValidator<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");
    }
}