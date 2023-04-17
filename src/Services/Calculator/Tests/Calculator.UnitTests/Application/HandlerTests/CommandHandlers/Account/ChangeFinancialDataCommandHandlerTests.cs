using Calculator.Application.Features.Account.Commands.ChangeFinancialData;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class ChangeFinancialDataCommandHandlerTests : CommandHandlerBaseTests<ChangeFinancialDataCommandHandler,
    ChangeFinancialDataCommand, Unit, Calculator.Domain.Account.Account>
{
}