﻿using Management.Application.Constants;
using Management.Application.Contracts.Repositories;
using MediatR;
using Shared.Implementations.Core.Exceptions;

namespace Management.Application.Features.Commands.Contract.UpdateHourlyRates;

public class UpdateHourlyRatesHandler : ICommandHandler<UpdateHourlyRatesCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContractRepository _contractRepository;

    public UpdateHourlyRatesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _contractRepository = unitOfWork.ContractRepository;
    }

    public async Task<Unit> Handle(UpdateHourlyRatesCommand request, CancellationToken cancellationToken)
    {
        var result = await _contractRepository.GetContractWithAccountByIdAsync(request.ContractId);
        if (result == null)
        {
            throw new NotFoundException(Messages.DataNotFoundMessage("Contract"),
                Titles.MethodFailedTitle("GetContractWithAccountByIdAsync"));
        }

        var contract = result.Map();

        contract.UpdateHourlyRates(request.HourlyRate, request.OvertimeRate, result.AccountId);

        await _contractRepository.UpdateHourlyRatesAsync(contract);
        await _unitOfWork.CompleteAsync(contract);

        return Unit.Value;
    }
}