using Calculator.Application.Features.Account.Commands.DeactivateAccount;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class DeactivateAccountHandlerTests : CommandHandlerBaseTests<DeactivateAccountCommandHandler,
    DeactivateAccountCommand, Unit, Domain.Account.Account>
{
}