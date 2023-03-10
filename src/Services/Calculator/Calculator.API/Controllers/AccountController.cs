using Calculator.Application.Features.Account.Commands.InitiationAccount;
using Calculator.Domain.Account;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Implementations.Abstractions;
using Shared.Implementations.EventStore.Repositories;
using Shared.Implementations.Snapshot;

namespace Calculator.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : BaseController
{
    private readonly ISnapshotStore _store;
    private readonly IRepository<Account> _repository;

    public AccountController(ICommandBus commandBus, IQueryBus queryBus, ISnapshotStore store, IRepository<Account> repository) : base(commandBus, queryBus)
    {
        _store = store;
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Test(InitiationAccountCommand command)
    {
        await CommandBus.Send(command);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> TestMongo([FromRoute]Guid id)
    {
        var result = await _repository.GetAsync(id);
        var resultSnapshot = result.CreateSnapshot();
        var serialize = JsonConvert.SerializeObject(resultSnapshot);
        await _store.AddAsync(new SnapshotState(id, resultSnapshot.Version, resultSnapshot.GetType().ToString(), serialize));

        var resultFromSn = await _repository.GetAggregateFromSnapshotAsync(id);
        return Ok(resultFromSn);
    }
}