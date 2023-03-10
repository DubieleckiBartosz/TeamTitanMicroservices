﻿using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Account.Commands.ChangeHourlyRate;

public class ChangeHourlyRateHandler : ICommandHandler<ChangeHourlyRateCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public ChangeHourlyRateHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<Unit> Handle(ChangeHourlyRateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}