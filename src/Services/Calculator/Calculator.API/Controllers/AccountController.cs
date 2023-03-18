using Calculator.Application.Features.Account.Commands.ActivateAccount;
using Calculator.Application.Features.Account.Commands.AddBonus;
using Calculator.Application.Features.Account.Commands.AddPieceProduct;
using Calculator.Application.Features.Account.Commands.AddWorkDay;
using Calculator.Application.Features.Account.Commands.CancelBonus;
using Calculator.Application.Features.Account.Commands.ChangeCountingType;
using Calculator.Application.Features.Account.Commands.ChangeDayHours;
using Calculator.Application.Features.Account.Commands.ChangeFinancialData;
using Calculator.Application.Features.Account.Commands.CompleteData;
using Calculator.Application.Features.Account.Commands.DeactivateAccount;
using Calculator.Application.Parameters;
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
    /// New counting type
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> ChangeCountingType([FromBody] ChangeCountingTypeParameters parameters)
    {
        var command = ChangeCountingTypeCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Change day hours
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> ChangeDayHours([FromBody] ChangeDayHoursParameters parameters)
    {
        var command = ChangeDayHoursCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Change financial data
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> ChangeFinancialData([FromBody] ChangeFinancialDataParameters parameters)
    {
        var command = ChangeFinancialDataCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Complete data
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> CompleteData([FromBody] CompleteDataParameters parameters)
    {
        var command = CompleteAccountDataCommand.Create(parameters);
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