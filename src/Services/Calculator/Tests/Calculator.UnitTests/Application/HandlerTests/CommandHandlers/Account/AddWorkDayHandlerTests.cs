using Calculator.Application.Features.Account.Commands.AddWorkDay;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class
    AddWorkDayHandlerTests : CommandHandlerBaseTests<AddWorkDayHandler, AddWorkDayCommand, Unit, Domain.Account.Account>
{
}