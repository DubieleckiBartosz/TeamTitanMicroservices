using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Bonus.Commands.AddDepartmentToBonus;

public record AddDepartmentToBonusCommand() : ICommand<Unit>
{
}