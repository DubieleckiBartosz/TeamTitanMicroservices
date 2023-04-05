using Management.Application.Features.Commands.Employee.AddContract;
using Management.Application.Features.Commands.Employee.AddDayOffRequest;
using Management.Application.Features.Commands.Employee.CreateEmployee;
using Management.Application.Features.Commands.Employee.UpdateAddressData;
using Management.Application.Features.Commands.Employee.UpdateContactData;
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

    [Authorize(Roles = "Admin,Owner,Manager,Employee")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateContactData([FromBody] UpdateContactDataParameters parameters)
    {
        var command = UpdateContactDataCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    [Authorize(Roles = "Admin,Owner,Manager,Employee")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateAddressData([FromBody] UpdateAddressDataParameters parameters)
    {
        var command = UpdateAddressDataCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeParameters parameters)
    {
        var command = CreateEmployeeCommand.Create(parameters);
        var result = await CommandBus.Send(command);
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Owner,Manager,Employee")]
    [HttpPost("[action]")]
    public async Task<IActionResult> NewDayOffRequest([FromBody] DayOffRequestParameters parameters)
    {
        var command = DayOffRequestCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpPost("[action]")]
    public async Task<IActionResult> NewEmployeeContract([FromBody] NewEmployeeContractParameters parameters)
    {
        var command = NewEmployeeContractCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }
}