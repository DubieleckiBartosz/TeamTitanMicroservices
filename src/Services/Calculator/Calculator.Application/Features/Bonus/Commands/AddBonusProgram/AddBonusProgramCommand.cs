using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Bonus.Commands.AddBonusProgram;

public record AddBonusProgramCommand() : ICommand<Unit>
{ 
}