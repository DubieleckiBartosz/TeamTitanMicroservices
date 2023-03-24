using Calculator.Application.Features.Account.Commands.AddPieceProduct;
using FluentValidation;
using Shared.Implementations.Validators;

namespace Calculator.Application.Validators.CommandValidators.AccountCommandValidators;

public class AddPieceProductCommandValidator : AbstractValidatorNotNull<AddPieceProductCommand>
{
    public AddPieceProductCommandValidator()
    {
        RuleFor(_ => _.AccountId).SetValidator(new GuidValidator());
        RuleFor(_ => _.PieceworkProductId).SetValidator(new GuidValidator());
        RuleFor(_ => _.Quantity).GreaterThan(0);
        RuleFor(_ => _.CurrentPrice).GreaterThan(0);
        RuleFor(_ => _.Date).Must(_ => _ > DateTime.UtcNow.AddMonths(-1).AddDays(-1) && _ <= DateTime.Today);
    }
}