using System.Data;
using Calculator.Application.Contracts;
using Calculator.Application.ReadModels.ProductReaders;
using Calculator.Infrastructure.DataAccessObjects.ProductDataAccessObjects;
using Dapper;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;
using Shared.Implementations.Search;

namespace Calculator.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<ProductRepository>, IProductRepository
{
    public ProductRepository(string dbConnection, ILoggerManager<ProductRepository> loggerManager) : base(dbConnection,
        loggerManager)
    {
    }

    public async Task<bool?> ProductSkuExistsAsync(string sku)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@productSku", sku);

        var result = (await QueryAsync<bool>("product_skuExists_S", parameters, CommandType.StoredProcedure))
            ?.FirstOrDefault();

        return result;
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

        var result = (await QueryAsync<ProductDao, PriceHistoryDao, ProductDao>("product_getWithHistoryById_S",
            (p, ph) =>
            {
                if (!dict.TryGetValue(p.Id, out var value))
                {
                    value = p;
                    dict.Add(p.Id, value);
                }

                value.PriceHistory.Add(ph);

                return value;
            }, "Id,Id", parameters, CommandType.StoredProcedure))?.FirstOrDefault();

        return result?.Map();
    }

    public async Task<ResponseSearchList<ProductReader>?> GetProductsBySearchAsync(string? productSku,
        decimal? pricePerUnitFrom, decimal? pricePerUnitTo,
        string? countedInUnit, string? productName, DateTime? fromDate, DateTime? toDate, bool isAvailable, string type,
        string name, int pageNumber, int pageSize, string companyCode)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@productSku", productSku);
        parameters.Add("@pricePerUnitFrom", pricePerUnitFrom);
        parameters.Add("@pricePerUnitTo", pricePerUnitTo);
        parameters.Add("@countedInUnit", countedInUnit);
        parameters.Add("@productName", productName);
        parameters.Add("@fromDate", fromDate);
        parameters.Add("@toDate", toDate);
        parameters.Add("@isAvailable", isAvailable);
        parameters.Add("@type", type);
        parameters.Add("@name", name);
        parameters.Add("@pageNumber", pageNumber);
        parameters.Add("@pageSize", pageSize);
        parameters.Add("@companyCode", companyCode);

        var result =
            (await QueryAsync<ProductSearchDao>("product_getBySearch_S", parameters, CommandType.StoredProcedure))
            ?.ToList();


        var totalCount = result?.FirstOrDefault()?.TotalCount;
        var data = result?.Select(_ => _.Map()).ToList();

        return ResponseSearchList<ProductReader>.Create(data, totalCount ?? 0);
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