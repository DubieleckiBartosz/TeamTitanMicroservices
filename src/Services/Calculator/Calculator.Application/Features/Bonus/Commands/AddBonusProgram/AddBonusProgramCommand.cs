using Calculator.Application.Parameters.Bonus;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Bonus.Commands.AddBonusProgram;

public record AddBonusProgramCommand(decimal BonusAmount, string CompanyCode, DateTime? Expires,
    string Reason) : ICommand<Guid>
{
    public AddBonusProgramCommand Create(AddBonusProgramParameters parameters)
    {
        return new AddBonusProgramCommand(parameters.BonusAmount, parameters.CompanyCode, parameters.Expires, parameters.Reason);
    }
}