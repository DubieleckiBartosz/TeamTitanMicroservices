using Calculator.Application.Contracts;
using Shared.Implementations.Dapper;
using Shared.Implementations.Logging;

namespace Calculator.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<ProductRepository>, IProductRepository
{
    public ProductRepository(string dbConnection, ILoggerManager<ProductRepository> loggerManager) : base(dbConnection, loggerManager)
    {
    }
}