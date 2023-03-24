using System.Data;
using Calculator.Application.Contracts;
using Calculator.Application.ReadModels.ProductReaders;
using Calculator.Infrastructure.DataAccessObjects.ProductDataAccessObjects;
using Dapper;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Calculator.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<ProductRepository>, IProductRepository
{
    public ProductRepository(string dbConnection, ILoggerManager<ProductRepository> loggerManager) : base(dbConnection,
        loggerManager)
    {
    }

    public async Task<ProductReader?> GetProductByIdAsync(Guid id)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@id", id);

        var result = (await this.QueryAsync<ProductDao>("product_getById_S", parameters, CommandType.StoredProcedure))?.FirstOrDefault();

        return result?.Map();
    }

    public async Task<ProductReader?> GetProductWithHistoryAsync(Guid id, string company)
    {
        var parameters = new DynamicParameters();

        var dict = new Dictionary<Guid, ProductDao>();
        parameters.Add("@id", id);
        parameters.Add("@company", company);

        var result = (await this.QueryAsync<ProductDao, PriceHistoryDao, ProductDao>("product_getWithHistoryById_S",(p,ph) =>
        {
            if (!dict.TryGetValue(p.Id, out var value))
            {
                value = p;
                dict.Add(p.Id, value);
            }
            value.PriceHistory.Add(ph);

            return value;
        },"Id,Id", parameters, CommandType.StoredProcedure))?.FirstOrDefault();

        return result?.Map();
    }

    public Task<List<ProductReader>?> GetProductsBySearchAsync()
    {
        //[TODO]
        throw new NotImplementedException();
    }

    public async Task AddAsync(ProductReader productReader)
    {
        var parameters = new DynamicParameters(); 

        parameters.Add("@id", productReader.Id);
        parameters.Add("@companyCode", productReader.CompanyCode); 
        parameters.Add("@productSku", productReader.ProductSku); 
        parameters.Add("@pricePerUnit", productReader.PricePerUnit);
        parameters.Add("@countedInUnit", productReader.CountedInUnit);
        parameters.Add("@productName", productReader.ProductName);
        parameters.Add("@isAvailable", productReader.IsAvailable);
        parameters.Add("@createdBy", productReader.CreatedBy);

        var result = await this.ExecuteAsync("product_createNew_I", parameters, CommandType.StoredProcedure);

        if (result <= 0)
        {
            throw new DatabaseException("Database operation failed", "Product creation failed");
        }
    }

    public async Task UpdatePriceAsync(ProductReader productReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@id", productReader.Id);
        parameters.Add("@newPricePerUnit", productReader.PricePerUnit); 

        var result = await this.ExecuteAsync("product_newPrice_U", parameters, CommandType.StoredProcedure);

        if (result <= 0)
        {
            throw new DatabaseException("Database operation failed", "Product update price failed");
        }
    }

    public async Task UpdateAvailabilityAsync(ProductReader productReader)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@id", productReader.Id);
        parameters.Add("@newAvailability", productReader.IsAvailable); 

        var result = await this.ExecuteAsync("product_newAvailability_U", parameters, CommandType.StoredProcedure);

        if (result <= 0)
        {
            throw new DatabaseException("Database operation failed", "Product update availability failed");
        }
    }
}