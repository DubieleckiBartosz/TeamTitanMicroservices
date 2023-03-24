using Calculator.Application.Contracts;
using Calculator.Application.Generators;
using Calculator.Domain.Product;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Services;

namespace Calculator.Application.Features.Product.Commands.CreateNewProduct;

public class CreateNewProductCommandHandler : ICommandHandler<CreateNewProductCommand, Unit>
{
    private readonly IRepository<PieceworkProduct> _repository;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUser _currentUser;

    public CreateNewProductCommandHandler(IRepository<PieceworkProduct> repository, IProductRepository productRepository, ICurrentUser currentUser)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }

    public async Task<Unit> Handle(CreateNewProductCommand request, CancellationToken cancellationToken)
    {
        var generate = true;
        var newSku = string.Empty;
        while (generate)
        {
            newSku = SkuGenerator.Generate();
            var skuExists = await _productRepository.ProductSkuExistsAsync(newSku);
            
            if (skuExists is null or false)
            {
                generate = false;
            }
        }

        var companyCode = _currentUser.OrganizationCode!;
        var userUniqueCode = _currentUser.VerificationCode!;
        var price = request.PricePerUnit;
        var countedInUnit = request.CountedInUnit;
        var productName = request.ProductName;

        var newProduct =
            PieceworkProduct.Create(companyCode, price, countedInUnit, productName, userUniqueCode, newSku);

        await _repository.AddAsync(newProduct);

        return Unit.Value;
    }
}