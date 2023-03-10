using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Account.Commands.AddWorkDay;

public class AddWorkDayHandler : ICommandHandler<AddWorkDayCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public AddWorkDayHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<Unit> Handle(AddWorkDayCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}