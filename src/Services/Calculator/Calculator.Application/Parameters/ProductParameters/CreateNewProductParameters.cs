using Newtonsoft.Json;

namespace Calculator.Application.Parameters.ProductParameters;

public class CreateNewProductParameters
{
    public decimal PricePerUnit { get; init; }
    public string CountedInUnit { get; init; }
    public string ProductName { get; init; }

    public CreateNewProductParameters()
    {
    }

    [JsonConstructor]
    public CreateNewProductParameters(decimal pricePerUnit, string countedInUnit, string productName)
    {
        PricePerUnit = pricePerUnit;
        CountedInUnit = countedInUnit;
        ProductName = productName; 
    }
}