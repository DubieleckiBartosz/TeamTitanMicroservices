using Calculator.Application.Constants;
using Calculator.Domain.Product;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Services;

namespace Calculator.Application.Features.Product.Commands.UpdateAvailability;

public class UpdateAvailabilityCommandHandler : ICommandHandler<UpdateAvailabilityCommand, Unit>
{
    private readonly IRepository<PieceworkProduct> _repository;
    private readonly ICurrentUser _currentUser;

    public UpdateAvailabilityCommandHandler(IRepository<PieceworkProduct> repository, ICurrentUser currentUser)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<Unit> Handle(UpdateAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAsync(request.ProductId);
        if (result == null || _currentUser.OrganizationCode != result.CompanyCode)
        {
            throw new NotFoundException(Messages.ProductNotFoundMessage, Titles.DataNotFoundTitle);
        }

        result.UpdateAvailability();
        await _repository.UpdateAsync(result);

        return Unit.Value;
    }
}