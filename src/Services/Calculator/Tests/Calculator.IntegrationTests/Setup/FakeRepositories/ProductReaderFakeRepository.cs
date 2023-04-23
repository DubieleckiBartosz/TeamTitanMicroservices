using AutoFixture;
using Calculator.Application.ReadModels.ProductReaders;
using Calculator.IntegrationTests.Generators.Product;

namespace Calculator.IntegrationTests.Setup.FakeRepositories;

public class ProductReaderFakeRepository
{
    private readonly Dictionary<ProductKey, List<EventReader<ProductReader>>> _productsDictionary;
    private readonly List<ProductReader> _products;

    public enum ProductKey
    {
        ProductCreated = 1, 
        AvailabilityUpdatedFalse = 2,
        PriceUpdated = 3
    }

    private readonly Fixture _fixture; 

    public ProductReaderFakeRepository(Fixture fixture)
    {
        _fixture = fixture;
        _productsDictionary = new Dictionary<ProductKey, List<EventReader<ProductReader>>>()
        {
            {ProductKey.ProductCreated, GetNewProducts()},
            {ProductKey.AvailabilityUpdatedFalse, GetNotAvailableProducts()},
            {ProductKey.PriceUpdated, GetProductsWithPriceHistory()}
        };

        _products = _productsDictionary.SelectMany(_ => _.Value.Select(_ => _.Reader)).ToList();
    }

    public EventReader<ProductReader> GetFirstAvailableProduct() => _productsDictionary[ProductKey.ProductCreated][0]; 
    public EventReader<ProductReader> GetFirstProductWithHistory() => _productsDictionary[ProductKey.PriceUpdated][0];
    public EventReader<ProductReader> GetFirstNotAvailableProduct() => _productsDictionary[ProductKey.AvailabilityUpdatedFalse][0];

    public IReadOnlyList<ProductReader> GetAllProducts()
    {
        return _products.AsReadOnly();
    }  

    private List<EventReader<ProductReader>> GetNewProducts(int count = 1)
    {
        var response = new List<EventReader<ProductReader>>();
        for (var i = 0; i < count; i++)
        {
            var @event = ProductEventGenerator.GetNewProductCreatedEvent(_fixture);
            var product = ProductReaderGenerator.GetNewProductReader(@event);

            var events = new List<IEvent>() { @event };
            response.Add(new EventReader<ProductReader>(product, events)); 
        }

        return response;
    }

    private List<EventReader<ProductReader>> GetNotAvailableProducts(int count = 1)
    {
        var response = new List<EventReader<ProductReader>>();
        for (var i = 0; i < count; i++)
        {
            var eventReaders = GetNewProducts(count);

            foreach (var eventReader in eventReaders)
            {
                var reader = eventReader.Reader;
                var @event = ProductEventGenerator.GetAvailabilityUpdatedEvent(false, reader.Id);
                var product = ProductReaderGenerator.GetProductReaderUpdatedAvailability(@event, reader);

                eventReader.Events.Add(@event); 

                response.Add(new EventReader<ProductReader>(product, eventReader.Events));
            }
        }

        return response;
    }

    private List<EventReader<ProductReader>> GetProductsWithPriceHistory(int count = 1)
    {
        var response = new List<EventReader<ProductReader>>();
        for (var i = 0; i < count; i++)
        {
            var eventReaders = GetNewProducts(count);

            foreach (var eventReader in eventReaders)
            {
                var reader = eventReader.Reader;
                var @event = ProductEventGenerator.GetProductPriceUpdatedEvent(_fixture, reader.Id);
                var product = ProductReaderGenerator.GetWithNewPriceProductReader(@event, reader);

                eventReader.Events.Add(@event);

                response.Add(new EventReader<ProductReader>(product, eventReader.Events));
            }
        }

        return response;
    }
}