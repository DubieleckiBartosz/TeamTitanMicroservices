using MediatR;

namespace Management.Application.Features.Commands.Contract.UpdatePaymentMonthDay;

public class UpdatePaymentMonthDayHandler : ICommandHandler<UpdatePaymentMonthDayCommand, Unit>
{
    public Task<Unit> Handle(UpdatePaymentMonthDayCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}