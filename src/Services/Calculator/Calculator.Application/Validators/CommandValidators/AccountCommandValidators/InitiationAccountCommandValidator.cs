using Calculator.Application.Features.Account.Commands.InitiationAccount;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class InitiationAccountCommandValidator : AbstractValidatorNotNull<InitiationAccountCommand>
{
    public InitiationAccountCommandValidator()
    {
        RuleFor(_ => _.CompanyCode).StringValidator();
        RuleFor(_ => _.AccountOwnerCode).StringValidator();
        RuleFor(_ => _.Creator).StringValidator();
    }
}