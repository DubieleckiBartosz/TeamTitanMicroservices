using Calculator.Application.Features.Product.Commands.CreateNewProduct;
using Calculator.Application.Features.Product.Commands.UpdateAvailability;
using Calculator.Application.Features.Product.Commands.UpdatePrice;
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
    /// Getting product with details
    /// </summary>
    /// <param name="productId"></param>
    /// <returns>ProductDetailsViewModel</returns>
    [Authorize(Roles = "Admin,Owner,Manager,Employee")]
    [HttpGet("[action]/{productId:guid}")]
    public async Task<IActionResult> GetProductById([FromRoute] Guid productId)
    {
        var query = GetProductDetailsByIdQuery.Create(productId);
        var response = await QueryBus.Send(query);
        return Ok(response);
    }
     
    /// <summary>
    /// Searching products by parameters
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

     
    /// <summary>
    /// Creating new product
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateNewProduct([FromBody] CreateNewProductParameters parameters)
    {
        var command = CreateNewProductCommand.Create(parameters);
        await CommandBus.Send(command);

        return NoContent();
    }

    /// <summary>
    /// Updating availability product
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateAvailability([FromBody] UpdateAvailabilityParameters parameters)
    {
        var command = UpdateAvailabilityCommand.Create(parameters.ProductId);
        await CommandBus.Send(command); 

        return NoContent();
    } 
    
    /// <summary>
    /// Updating price  
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdatePriceProduct([FromBody] UpdatePriceParameters parameters)
    {
        var command = UpdatePriceCommand.Create(parameters);
        await CommandBus.Send(command); 

        return NoContent();
    }
}