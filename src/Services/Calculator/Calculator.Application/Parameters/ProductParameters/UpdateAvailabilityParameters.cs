using Newtonsoft.Json;

namespace Calculator.Application.Parameters.ProductParameters;

public class UpdateAvailabilityParameters
{
    public Guid ProductId { get; init; }

    [JsonConstructor]
    public UpdateAvailabilityParameters(Guid productId)
    {
        ProductId = productId;
    }
}