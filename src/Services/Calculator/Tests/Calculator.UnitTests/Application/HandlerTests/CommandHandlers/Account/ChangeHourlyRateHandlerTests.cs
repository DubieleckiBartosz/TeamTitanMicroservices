using Calculator.Application.Features.Account.Commands.ChangeHourlyRate;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class ChangeHourlyRateHandlerTests : CommandHandlerBaseTests<ChangeHourlyRateCommandHandler,
    ChangeHourlyRateCommand, Unit, Domain.Account.Account>
{
}