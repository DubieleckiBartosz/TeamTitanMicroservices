using Calculator.Application.Features.Account.Commands.ActivateAccount;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class ActivateAccountHandlerTests : CommandHandlerBaseTests<ActivateAccountCommandHandler, ActivateAccountCommand
    , Unit, Domain.Account.Account>
{
}