using Shared.Domain.Abstractions;

namespace Calculator.Domain.Product.Events;

public record NewProductCreated(string CompanyCode, decimal PricePerUnit, string CountedInUnit, string ProductName,
    string CreatedBy, string ProductCode, Guid ProductId) : IEvent
{
    public static NewProductCreated Create(string companyCode, decimal pricePerUnit, string countedInUnit,
        string productName,
        string createdBy, string productCode, Guid productId)
    {
        return new NewProductCreated(companyCode, pricePerUnit, countedInUnit, productName, createdBy, productCode,
            productId);
    }
}