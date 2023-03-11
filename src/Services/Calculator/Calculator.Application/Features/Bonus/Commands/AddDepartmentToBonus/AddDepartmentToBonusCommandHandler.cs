using Calculator.Domain.BonusProgram;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;

namespace Calculator.Application.Features.Bonus.Commands.AddDepartmentToBonus;

public class AddDepartmentToBonusCommandHandler : ICommandHandler<AddDepartmentToBonusCommand, Unit>
{
    private readonly IRepository<BonusProgram> _repository;

    public AddDepartmentToBonusCommandHandler(IRepository<BonusProgram> repository)
    {
        _repository = repository;
    }

    public Task<Unit> Handle(AddDepartmentToBonusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}