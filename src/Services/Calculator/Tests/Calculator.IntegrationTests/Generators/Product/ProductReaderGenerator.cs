using Calculator.Application.ReadModels.ProductReaders;
using Calculator.Domain.Product.Events;

namespace Calculator.IntegrationTests.Generators.Product;

public class ProductReaderGenerator
{
    public static ProductReader GetNewProductReader(NewProductCreated @event)
    {
        return ProductReader.Create(@event);
    }

    public static ProductReader GetWithNewPriceProductReader(ProductPriceUpdated @event, ProductReader productReader)
    {
        productReader.PriceUpdated(@event);
        return productReader;
    }

    public static ProductReader GetProductReaderUpdatedAvailability(AvailabilityUpdated @event, ProductReader productReader)
    {
        productReader.NewAvailability(@event);
        return productReader;
    }

}