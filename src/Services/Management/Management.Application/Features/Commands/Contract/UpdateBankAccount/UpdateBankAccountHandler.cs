using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Core.Exceptions;

namespace Management.Application.Features.Commands.Contract.UpdateBankAccount;

public class UpdateBankAccountHandler : ICommandHandler<UpdateBankAccountCommand, Unit>
{ 
    private readonly IContractRepository _contractRepository;

    public UpdateBankAccountHandler(IUnitOfWork unitOfWork)
    { 
        _contractRepository = unitOfWork?.ContractRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<Unit> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var contract = (await _contractRepository.GetContractByIdAsync(request.ContractId))?.Map();
        if (contract == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Contract"),
                Titles.MethodFailedTitle("GetContractByIdAsync"));
        } 

        contract.AddOrUpdateBankAccountNumber(request.BankAccountNumber);

        await _contractRepository.UpdateBankAccountNumberAsync(contract);

        return Unit.Value;
    }
}