using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Validators;

namespace Calculator.Application.Features.Account.Commands.AddPieceProduct;

public class AddPieceProductHandler : ICommandHandler<AddPieceProductCommand, Unit>
{
    private readonly IRepository<Domain.Account.Account> _repository;

    public AddPieceProductHandler(IRepository<Domain.Account.Account> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Unit> Handle(AddPieceProductCommand request, CancellationToken cancellationToken)
    {
        var account = await _repository.GetAsync(request.AccountId);

        account.CheckAndThrowWhenNull("Account");

        var pieceworkProductId = request.PieceworkProductId;
        var quantity = request.Quantity;
        var currentPrice = request.CurrentPrice;
        var date = request.Date;

        account.AddNewPieceProductItem(pieceworkProductId, quantity, currentPrice, date);

        await _repository.UpdateAsync(account);

        return Unit.Value;
    }
}