using AutoFixture;
using Calculator.Application.Parameters.ProductParameters;

namespace Calculator.IntegrationTests.Generators.Product;

public static class ProductRequestsGenerator
{
    public static CreateNewProductParameters GetCreateNewProductParameters(this Fixture fixture)
    {
        return fixture.Create<CreateNewProductParameters>();
    }

    public static UpdateAvailabilityParameters GetUpdateAvailabilityParameters(this Fixture fixture, Guid productId)
    {
        return fixture.Build<UpdateAvailabilityParameters>()
            .With(w => w.ProductId, productId).Create();
    }

    public static UpdatePriceParameters GetUpdatePriceParameters(this Fixture fixture, Guid productId)
    {
        return fixture.Build<UpdatePriceParameters>()
            .With(w => w.ProductId, productId).Create();
    }
}