using Calculator.Application.Features.Account.Commands.ChangeOvertimeRate;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class ChangeOvertimeRateHandlerTests : CommandHandlerBaseTests<ChangeOvertimeRateCommandHandler,
    ChangeOvertimeRateCommand, Unit, Domain.Account.Account>
{
}