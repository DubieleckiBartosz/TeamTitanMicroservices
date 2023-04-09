using Management.Application.Features.Commands.Contract.UpdateBankAccount;
using Management.Application.Features.Commands.Contract.UpdateHourlyRates;
using Management.Application.Features.Commands.Contract.UpdatePaymentMonthDay;
using Management.Application.Features.Commands.Contract.UpdateSalary;
using Management.Application.Features.Commands.Contract.UpdateSettlementType;
using Management.Application.Parameters.ContractParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Implementations.Abstractions;

namespace Management.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContractController : BaseController
{
    public ContractController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }

    /// <summary>
    /// Changing bank account number
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateBankAccount([FromBody] UpdateBankAccountParameters parameters)
    {
        var command = UpdateBankAccountCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Changing hourly rate or overtime rate
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateHourlyRates([FromBody] UpdateHourlyRatesParameters parameters)
    {
        var command = UpdateHourlyRatesCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Changing payment day
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdatePaymentMonthDay([FromBody] UpdatePaymentMonthDayParameters parameters)
    {
        var command = UpdatePaymentMonthDayCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Changing salary
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateSalary([FromBody] UpdateSalaryParameters parameters)
    {
        var command = UpdateSalaryCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Changing settlement type
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateLeader([FromBody] UpdateSettlementTypeParameters parameters)
    {
        var command = UpdateSettlementTypeCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }
}