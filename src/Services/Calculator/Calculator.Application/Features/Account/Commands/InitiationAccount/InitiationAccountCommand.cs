﻿using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.InitiationAccount;

public record InitiationAccountCommand(string CompanyCode, string AccountOwnerCode, string Creator) : ICommand<Unit>
{
}