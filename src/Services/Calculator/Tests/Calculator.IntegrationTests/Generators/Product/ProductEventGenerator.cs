using AutoFixture;
using Calculator.Domain.Product.Events;
using Calculator.IntegrationTests.Settings;

namespace Calculator.IntegrationTests.Generators.Product;

public class ProductEventGenerator
{
    public static NewProductCreated GetNewProductCreatedEvent(Fixture fixture)
    {
        var companyCode = GlobalSettings.OrganizationCode;
        var createdBy = GlobalSettings.UserVerificationCode;
        var pricePerUnit = fixture.Create<decimal>();
        var countedInUnit = fixture.Create<string>();
        var productName = fixture.Create<string>();
        var productSku = fixture.Create<string>(); 

        return NewProductCreated.Create(companyCode, pricePerUnit, countedInUnit, productName, createdBy, productSku, Guid.NewGuid());
    }

    public static AvailabilityUpdated GetAvailabilityUpdatedEvent(bool newAvailability = true, Guid? productId = null)
    {
        return AvailabilityUpdated.Create(newAvailability, productId ?? Guid.NewGuid());
    }
    
    public static ProductPriceUpdated GetProductPriceUpdatedEvent(Fixture fixture, Guid? productId = null)
    {
        var newPrice = fixture.Create<decimal>();
         
        return ProductPriceUpdated.Create(newPrice, productId ?? Guid.NewGuid());
    }
}