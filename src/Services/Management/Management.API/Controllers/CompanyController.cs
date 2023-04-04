using Management.Application.Features.Commands.Company.CompleteData;
using Management.Application.Features.Commands.Company.InitCompany;
using Management.Application.Features.Commands.Company.UpdateContact;
using Management.Application.Parameters.CompanyParameters;
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

    [Authorize(Roles = "Admin,Owner")]
    [HttpPut("[action]")]
    public async Task<IActionResult> CompleteCompanyData([FromBody] CompleteDataParameters parameters)
    {
        var command = CompleteDataCommand.Create(parameters);
        var result = await CommandBus.Send(command);

        return Ok(result);
    }

    [Authorize(Roles = "Admin,Owner")]
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateCommunicationData(
        [FromBody] UpdateCompanyContactParameters parameters)
    {
        var command = UpdateCompanyContactCommand.Create(parameters);
        var result = await CommandBus.Send(command);

        return Ok(result);
    }


    [Authorize(Roles = "Admin,User")]
    [HttpPost("[action]")]
    public async Task<IActionResult> InitCompany()
    {
        var command = new InitCompanyCommand();
        var result = await CommandBus.Send(command);

        return Ok(result);
    } 
}