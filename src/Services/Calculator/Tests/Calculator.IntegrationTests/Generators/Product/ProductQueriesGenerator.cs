using AutoFixture;
using Calculator.Application.Parameters.ProductParameters;

namespace Calculator.IntegrationTests.Generators.Product;

public static class ProductQueriesGenerator
{
    public static GetProductsBySearchParameters GetSearchProductsParameters(this Fixture fixture)
    {
        return fixture.Create<GetProductsBySearchParameters>();
    }
}