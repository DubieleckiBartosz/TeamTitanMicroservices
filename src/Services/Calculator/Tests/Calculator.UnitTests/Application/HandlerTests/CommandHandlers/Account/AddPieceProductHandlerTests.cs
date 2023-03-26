using Calculator.Application.Features.Account.Commands.AddPieceProduct;
using MediatR;

namespace Calculator.UnitTests.Application.HandlerTests.CommandHandlers.Account;

public class AddPieceProductHandlerTests : CommandHandlerBaseTests<AddPieceProductCommandHandler, AddPieceProductCommand
    , Unit, Domain.Account.Account>
{
}