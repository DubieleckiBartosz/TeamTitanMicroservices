using Calculator.Application.Features.Account.Commands.AccountSettlement;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class AccountSettlementCommandValidator : AbstractValidatorNotNull<AccountSettlementCommand>
{
    public AccountSettlementCommandValidator()
    {
        RuleFor(_ => _.AccountId).SetValidator(new GuidValidator());
    }
}