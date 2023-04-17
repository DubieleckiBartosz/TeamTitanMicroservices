using Calculator.Application.Features.Account.Commands.InitiationAccount;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class InitiationAccountHandlerTests : CommandHandlerBaseTests<InitiationAccountCommandHandler,
    InitiationAccountCommand, Unit, Calculator.Domain.Account.Account>
{
}