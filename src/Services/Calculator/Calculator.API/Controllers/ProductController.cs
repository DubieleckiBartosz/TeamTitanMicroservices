using Calculator.Application.Features.Product.Queries.GetProductDetailsById;
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
    public async Task<IActionResult> SearchAccounts([FromRoute] Guid productId)
    {
        var query = GetProductDetailsByIdQuery.Create(productId);
        var response = await QueryBus.Send(query);
        return Ok(response);
    }
}