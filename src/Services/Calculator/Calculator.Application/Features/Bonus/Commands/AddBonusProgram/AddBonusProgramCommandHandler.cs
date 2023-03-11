using Calculator.Domain.BonusProgram;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Bonus.Commands.AddBonusProgram;

public class AddBonusProgramCommandHandler : ICommandHandler<AddBonusProgramCommand, Unit>
{
    private readonly IRepository<BonusProgram> _repository;

    public AddBonusProgramCommandHandler(IRepository<BonusProgram> repository)
    {
        _repository = repository;
    }

    public Task<Unit> Handle(AddBonusProgramCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}