using Calculator.Application.Features.Account.Commands.CancelBonus;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class CancelBonusAccountCommandHandlerTests : CommandHandlerBaseTests<CancelBonusAccountCommandHandler,
    CancelBonusAccountCommand, Unit, Domain.Account.Account>
{
}