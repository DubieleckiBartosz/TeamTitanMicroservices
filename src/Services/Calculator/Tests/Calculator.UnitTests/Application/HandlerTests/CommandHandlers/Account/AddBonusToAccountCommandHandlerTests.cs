using Calculator.Application.Features.Account.Commands.AddBonus;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class AddBonusToAccountCommandHandlerTests : CommandHandlerBaseTests<AddBonusToAccountCommandHandler,
    AddBonusToAccountCommand, Unit, Domain.Account.Account>
{
}