using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdateFinancialData;

public class UpdateFinancialDataHandler : ICommandHandler<UpdateFinancialDataCommand, Unit>
{
    public Task<Unit> Handle(UpdateFinancialDataCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}