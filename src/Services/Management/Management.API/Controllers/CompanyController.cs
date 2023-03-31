using Management.Application.Features.Company.Commands.InitCompany;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Implementations.Abstractions;

namespace Management.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : BaseController
{
    public CompanyController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }

    [Authorize(Roles = "User")]
    [HttpPost("[action]")]
    public async Task<IActionResult> InitCompany()
    {
        var command = new InitCompanyCommand();
        var result = await CommandBus.Send(command);

        return Ok(result);
    }
}