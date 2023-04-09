using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdateSettlementType;

public class UpdateSettlementTypeHandler : ICommandHandler<UpdateSettlementTypeCommand, Unit>
{
    public Task<Unit> Handle(UpdateSettlementTypeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}