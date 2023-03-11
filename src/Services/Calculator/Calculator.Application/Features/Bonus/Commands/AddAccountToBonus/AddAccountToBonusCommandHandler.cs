using Calculator.Domain.BonusProgram;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Bonus.Commands.AddAccountToBonus;

public class AddAccountToBonusCommandHandler : ICommandHandler<AddAccountToBonusCommand, Unit>
{
    private readonly IRepository<BonusProgram> _repository;

    public AddAccountToBonusCommandHandler(IRepository<BonusProgram> repository)
    {
        _repository = repository;
    }
    public Task<Unit> Handle(AddAccountToBonusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}