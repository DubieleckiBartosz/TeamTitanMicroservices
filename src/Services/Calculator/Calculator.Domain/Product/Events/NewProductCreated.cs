using Shared.Domain.Abstractions;

namespace Calculator.Domain.Product.Events;

public record NewProductCreated(string CompanyCode, decimal PricePerUnit, string CountedInUnit, string ProductName,
    string CreatedBy, string ProductSku, Guid ProductId) : IEvent
{
    public static NewProductCreated Create(string companyCode, decimal pricePerUnit, string countedInUnit,
        string productName,
        string createdBy, string productSku, Guid productId)
    {
        return new NewProductCreated(companyCode, pricePerUnit, countedInUnit, productName, createdBy, productSku,
            productId);
    }
}