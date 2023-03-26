using Calculator.Application.Features.Account.Commands.ChangeDayHours;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class ChangeDayHoursHandlerTests : CommandHandlerBaseTests<ChangeDayHoursCommandHandler, ChangeDayHoursCommand,
    Unit, Domain.Account.Account>
{
}