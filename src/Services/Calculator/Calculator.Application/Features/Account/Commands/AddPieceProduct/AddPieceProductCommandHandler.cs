using Calculator.Domain.Account.Snapshots;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Services;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.AddPieceProduct;

public class AddPieceProductCommandHandler : ICommandHandler<AddPieceProductCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;
    private readonly ICurrentUser _currentUser;

    public AddPieceProductCommandHandler(IRepository<Domain.Account.Account> repository, ICurrentUser currentUser)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    public async Task<Unit> Handle(AddPieceProductCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAggregateFromSnapshotAsync<AccountSnapshot>(request.AccountId);

        account.CheckAndThrowWhenNullOrNotMatch("Account", _ => _.Details.CompanyCode == _currentUser.OrganizationCode);

        var pieceworkProductId = request.PieceworkProductId;
        var quantity = request.Quantity;
        var currentPrice = request.CurrentPrice;
        var date = request.Date;

        account.AddNewPieceProductItem(pieceworkProductId, quantity, currentPrice, date ?? DateTime.UtcNow);

        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}