using Management.Application.Parameters.ContractParameters;
using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdateBankAccount;

public record UpdateBankAccountCommand(int ContractId, string BankAccountNumber) : ICommand<Unit>
{
    public static UpdateBankAccountCommand Create(UpdateBankAccountParameters parameters)
    {
        return new UpdateBankAccountCommand(parameters.ContractId, parameters.BankAccountNumber);
    }
}