﻿using Management.Application.Parameters.ContractParameters;
using Management.Application.ValueTypes;
using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdateSettlementType;

public record UpdateSettlementTypeCommand(int ContractId, SettlementType NewSettlementType) : ICommand<Unit>
{
    public static UpdateSettlementTypeCommand Create(UpdateSettlementTypeParameters parameters)
    {
        return new UpdateSettlementTypeCommand(parameters.ContractId, parameters.NewSettlementType);
    }
}