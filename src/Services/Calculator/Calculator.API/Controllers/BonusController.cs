using Microsoft.AspNetCore.Mvc;
using Shared.Implementations.Abstractions;

namespace Calculator.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BonusController : BaseController
{
    public BonusController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }
}