using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Bonus.Commands.AddAccountToBonus;

public record AddAccountToBonusCommand() : ICommand<Unit>
{
}