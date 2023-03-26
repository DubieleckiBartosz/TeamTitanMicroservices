using Calculator.Application.Features.Account.Commands.UpdateData;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class UpdateAccountDataHandlerTests : CommandHandlerBaseTests<UpdateAccountDataCommandHandler,
    UpdateAccountDataCommand, Unit, Domain.Account.Account>
{
}