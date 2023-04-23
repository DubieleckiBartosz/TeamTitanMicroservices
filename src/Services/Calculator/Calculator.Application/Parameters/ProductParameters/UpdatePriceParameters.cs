using Newtonsoft.Json;

namespace Calculator.Application.Parameters.ProductParameters;

public class UpdatePriceParameters
{
    public Guid ProductId { get; init; }
    public decimal Price { get; init; }

    public UpdatePriceParameters()
    {
    }

    [JsonConstructor]
    public UpdatePriceParameters(Guid productId, decimal price)
    {
        ProductId = productId;
        Price = price;
    }
}