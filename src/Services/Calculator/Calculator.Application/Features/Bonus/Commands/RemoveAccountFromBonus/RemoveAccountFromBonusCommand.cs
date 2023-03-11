using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Bonus.Commands.RemoveAccountFromBonus;

public record RemoveAccountFromBonusCommand() : ICommand<Unit>
{ 
}