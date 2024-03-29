﻿using Calculator.Domain.Types;
using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.ChangeCountingType;

public record ChangeCountingTypeCommand(CountingType NewCountingType, Guid AccountId) : ICommand<Unit>
{
    public static ChangeCountingTypeCommand Create(CountingType newCountingType, Guid accountId)
    {
        return new ChangeCountingTypeCommand(newCountingType, accountId);
    }
}