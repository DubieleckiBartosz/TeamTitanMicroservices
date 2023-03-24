using Calculator.Application.Contracts;
using Calculator.Application.ReadModels.ProductReaders; 
using Calculator.Domain.Product.Events;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Projection;

namespace Calculator.Application.Projections;

public class ProductProjection : ReadModelAction<ProductReader>
{
    private readonly IProductRepository _productRepository; 
    public ProductProjection(IProductRepository productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));

        this.Projects<NewProductCreated>(Handle);
        this.Projects<ProductPriceUpdated>(Handle);
        this.Projects<AvailabilityUpdated>(Handle);
    }

    private async Task Handle(NewProductCreated @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var productReader = ProductReader.Create(@event);
        await _productRepository.AddAsync(productReader);
    }

    private async Task Handle(ProductPriceUpdated @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var productResult = await _productRepository.GetProductByIdAsync(@event.ProductId);

        this.CheckAccount(productResult);

        productResult!.PriceUpdated(@event);
        await _productRepository.UpdatePriceAsync(productResult!);
    }

    private async Task Handle(AvailabilityUpdated @event, CancellationToken cancellationToken = default)
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var productResult = await _productRepository.GetProductByIdAsync(@event.ProductId);

        this.CheckAccount(productResult);

        productResult!.NewAvailability(@event);
        await _productRepository.UpdateAvailabilityAsync(productResult!);
    }

    private void CheckAccount(ProductReader? product)
    {
        if (product == null)
        {
            throw new NotFoundException("Product cannot be null.", "Product not found");
        }
    }
}