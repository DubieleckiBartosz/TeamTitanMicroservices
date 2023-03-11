using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Bonus.Commands.RemoveDepartmentFromBonus;

public record RemoveDepartmentFromBonusCommand() : ICommand<Unit>
{ 
}