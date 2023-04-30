using General.Application.Features.Comments.Commands.CreateComment;
using General.Application.Features.Comments.Commands.DeleteComment;
using General.Application.Features.Comments.Commands.UpdateComment;
using General.Application.Features.Comments.Queries.SearchComments;
using General.Application.Features.Reaction.CreateCommentReaction;
using General.Application.Features.Reaction.CreatePostReaction;
using General.Application.Features.Reaction.DeletePostReaction;
using General.Application.Models.Parameters.CommentParameters;
using General.Application.Models.Parameters.PostParameters;
using General.Application.Models.Parameters.SearchParameters;
using Microsoft.AspNetCore.Mvc;
using Shared.Implementations.Abstractions;

namespace General.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController : BaseController
{
    public CommentController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> SearchComments([FromQuery] SearchCommentsParameters parameters)
    {
        var query = SearchCommentsQuery.Create(parameters);
        var response = await QueryBus.Send(query);
        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateNewPost([FromBody] CreateCommentParameters parameters)
    {
        var command = CreateCommentCommand.Create(parameters);
        var response = await CommandBus.Send(command);
        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateCommentReaction([FromBody] CreateCommentReactionParameters parameters)
    {
        var command = CreateCommentReactionCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentParameters parameters)
    {
        var command = UpdateCommentCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteComment([FromBody] DeleteCommentParameters parameters)
    {
        var command = DeleteCommentCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteCommentReaction([FromBody] DeleteCommentParameters parameters)
    {
        var command = DeleteCommentCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }
}