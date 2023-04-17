using Calculator.Application.Features.Account.Commands.ChangeCountingType;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class ChangeCountingTypeHandlerTests : CommandHandlerBaseTests<ChangeCountingTypeCommandHandler,
    ChangeCountingTypeCommand, Unit, Calculator.Domain.Account.Account>
{
}