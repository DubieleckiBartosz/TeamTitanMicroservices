using Management.Application.Features.Commands.DayOffRequest.AddDayOffRequest;
using Management.Application.Features.Commands.DayOffRequest.CancelDayOffRequest;
using Management.Application.Features.Commands.DayOffRequest.ConsiderDayOffRequest;
using Management.Application.Features.Commands.Employee.AddContract;
using Management.Application.Features.Commands.Employee.CreateEmployee;
using Management.Application.Features.Commands.Employee.UpdateAddressData;
using Management.Application.Features.Commands.Employee.UpdateContactData;
using Management.Application.Features.Commands.Employee.UpdateEmployeeLeader;
using Management.Application.Parameters.DayOffRequestParameters;
using Management.Application.Parameters.EmployeeParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Implementations.Abstractions;

namespace Management.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : BaseController
{
    public EmployeeController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }

    /// <summary>
    /// Changing contact data
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager,Employee")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateContactData([FromBody] UpdateContactDataParameters parameters)
    {
        var command = UpdateContactDataCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }
    
    /// <summary>
    /// Changing leader
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateLeader([FromBody] UpdateEmployeeLeaderParameters parameters)
    {
        var command = UpdateEmployeeLeaderCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Changing address
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager,Employee")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateAddressData([FromBody] UpdateAddressDataParameters parameters)
    {
        var command = UpdateAddressDataCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Canceling a request for a day off
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Employee")]
    [HttpPut("[action]")]
    public async Task<IActionResult> CancelDayOffRequest([FromBody] CancelDayOffRequestParameters parameters)
    {
        var command = CancelDayOffRequestCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Consideration of the request for a day off
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPut("[action]")]
    public async Task<IActionResult> ConsiderDayOffRequest([FromBody] ConsiderDayOffRequestParameters parameters)
    {
        var command = ConsiderDayOffRequestCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Creating new employee
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns>string</returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeParameters parameters)
    {
        var command = CreateEmployeeCommand.Create(parameters);
        var result = await CommandBus.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Creating a new request for a day off
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager,Employee")]
    [HttpPost("[action]")]
    public async Task<IActionResult> NewDayOffRequest([FromBody] NewDayOffRequestParameters parameters)
    {
        var command = DayOffRequestCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Creating a new contract for an employee
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPost("[action]")]
    public async Task<IActionResult> NewEmployeeContract([FromBody] NewEmployeeContractParameters parameters)
    {
        var command = NewEmployeeContractCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }
}