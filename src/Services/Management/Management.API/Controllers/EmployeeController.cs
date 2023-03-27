﻿using Microsoft.AspNetCore.Mvc;
using Shared.Implementations.Abstractions;

namespace Management.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : BaseController
{
    public EmployeeController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }
}