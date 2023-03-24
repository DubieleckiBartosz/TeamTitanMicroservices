using Calculator.Application.Features.Product.Queries.GetProductDetailsById;
using Calculator.Application.Features.Product.Queries.GetProductsBySearch;
using Calculator.Application.Parameters.ProductParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Implementations.Abstractions;

namespace Calculator.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : BaseController
{
    public ProductController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }

    /// <summary>
    /// Get product with details
    /// </summary>
    /// <param name="productId"></param>
    /// <returns>ProductDetailsViewModel</returns>
    [Authorize(Roles = "Admin,Owner,Manager,Employee")]
    [HttpPost("[action]/{productId:guid}")]
    public async Task<IActionResult> GetProductById([FromRoute] Guid productId)
    {
        var query = GetProductDetailsByIdQuery.Create(productId);
        var response = await QueryBus.Send(query);
        return Ok(response);
    }
     
    /// <summary>
    /// Find products by search
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns>List of ProductViewModel</returns>
    [Authorize(Roles = "Admin,Owner,Manager,Employee")]
    [HttpPost("[action]")]
    public async Task<IActionResult> SearchProducts([FromBody] GetProductsBySearchParameters parameters)
    {
        var query = GetProductsBySearchQuery.Create(parameters);
        var response = await QueryBus.Send(query);
        return Ok(response);
    }

     
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateNewProduct([FromBody] GetProductsBySearchParameters parameters)
    {
        var query = GetProductsBySearchQuery.Create(parameters);
        var response = await QueryBus.Send(query);
        return Ok(response);
    }
}