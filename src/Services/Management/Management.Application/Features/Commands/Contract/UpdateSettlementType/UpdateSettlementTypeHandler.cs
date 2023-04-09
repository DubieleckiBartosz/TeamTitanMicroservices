using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Core.Exceptions;

namespace Management.Application.Features.Commands.Contract.UpdateSettlementType;

public class UpdateSettlementTypeHandler : ICommandHandler<UpdateSettlementTypeCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContractRepository _contractRepository;

    public UpdateSettlementTypeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _contractRepository = unitOfWork.ContractRepository;
    }
    public async Task<Unit> Handle(UpdateSettlementTypeCommand request, CancellationToken cancellationToken)
    {
        var result = await _contractRepository.GetContractWithAccountByIdAsync(request.ContractId);
        if (result == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Contract"),
                Titles.MethodFailedTitle("GetContractWithAccountByIdAsync"));
        }

        var contract = result.Map();

        contract.UpdateSettlementType((int) request.NewSettlementType, result.AccountId);

        await _contractRepository.UpdateSettlementTypeAsync(contract);

        await _unitOfWork.CompleteAsync(contract);

        return Unit.Value;
    }
}