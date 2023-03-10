using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ChangeOvertimeRate;

public record ChangeOvertimeRateCommand(decimal NewOvertimeRate, Guid AccountId) : ICommand<Unit>
{
}