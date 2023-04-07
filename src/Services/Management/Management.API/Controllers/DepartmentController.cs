using Management.Application.Features.Commands.Department.CreateDepartment;
using Management.Application.Features.Queries.Department.GetDepartmentsByCompanyId;
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

    /// <summary>
    /// Get departments by company identifier
    /// </summary> 
    /// <returns>list of DepartmentViewModel</returns>
    [Authorize(Roles = "Admin,Owner,Manager")]
    [HttpGet("[action]/{companyId}")]
    public async Task<IActionResult> GetDepartmentsCompanyId([FromRoute] int companyId)
    {
        var query = DepartmentsByCompanyIdQuery.Create(companyId);
        var response = await QueryBus.Send(query);
        return Ok(response);
    }

    /// <summary>
    /// Creating new department
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [Authorize(Roles = "Admin,Owner")]
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentParameters parameters)
    {
        var command = CreateDepartmentCommand.Create(parameters);
        await CommandBus.Send(command); 
        return NoContent();
    } 
}