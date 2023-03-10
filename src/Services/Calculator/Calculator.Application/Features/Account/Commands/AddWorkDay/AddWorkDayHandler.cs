using MediatR;
using Shared.Implementations.Abstractions;

namespace Calculator.Application.Features.Account.Commands.AddWorkDay;

public class AddWorkDayHandler : ICommandHandler<AddWorkDayCommand, Unit>
{
    public Task<Unit> Handle(AddWorkDayCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}