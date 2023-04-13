using Calculator.Application.Features.Account.Commands.ActivateAccount;
using Calculator.Application.Features.Account.Commands.AddBonus;
using Calculator.Application.Features.Account.Commands.AddPieceProduct;
using Calculator.Application.Features.Account.Commands.AddWorkDay;
using Calculator.Application.Features.Account.Commands.CancelBonus;
using Calculator.Application.Features.Account.Commands.DeactivateAccount;
using Calculator.Application.Features.Account.Queries.GetAccountsBySearch;
using Calculator.Application.Parameters.AccountParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Implementations.Abstractions;

namespace Calculator.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AccountController : BaseController
{
    public AccountController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }

 
    /// <summary>
    /// Search accounts
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPost("[action]")]
    public async Task<IActionResult> SearchAccounts([FromBody] GetAccountsBySearchParameters parameters)
    {
        var query = GetAccountsBySearchQuery.Create(parameters);
        var response = await QueryBus.Send(query);
        return Ok(response);
    }
     
    /// <summary>
    /// Add new bonus to account
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPost("[action]")]
    public async Task<IActionResult> AddBonusToAccount([FromBody] AddBonusToAccountParameters parameters)
    {
        var command = AddBonusToAccountCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    ///  Add product to account
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPost("[action]")]
    public async Task<IActionResult> AddPieceProduct([FromBody] AddPieceProductParameters parameters)
    {
        var command = AddPieceProductCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Add work day
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPost("[action]")]
    public async Task<IActionResult> AddWorkDay([FromBody] AddWorkDayParameters parameters)
    {
        var command = AddWorkDayCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Activate account
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> ActivateAccount([FromBody] ActivateAccountParameters parameters)
    {
        var command = ActivateAccountCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Cancel bonus
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> CancelBonus([FromBody] CancelBonusAccountParameters parameters)
    {
        var command = CancelBonusAccountCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }  
     
    /// <summary>
    /// Deactivate account
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> DeactivateAccount([FromBody] DeactivateAccountParameters parameters)
    {
        var command = DeactivateAccountCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }  
}