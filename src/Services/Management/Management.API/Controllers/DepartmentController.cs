using Management.Application.Features.Commands.Department.CreateDepartment;
using Management.Application.Parameters.DepartmentParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Implementations.Abstractions;

namespace Management.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentController : BaseController
{
    public DepartmentController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }

    [Authorize(Roles = "Admin,Owner")]
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentParameters parameters)
    {
        var command = CreateDepartmentCommand.Create(parameters);
        await CommandBus.Send(command); 
        return NoContent();
    } 
}