using Calculator.Domain.Types;

namespace Calculator.Application.Features.Account.Commands.ChangeCountingType;

public record ChangeCountingTypeCommand(CountingType NewCountingType, Guid AccountId)
{
}