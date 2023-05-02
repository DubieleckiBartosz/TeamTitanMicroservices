using General.Application.Features.Posts.Commands.CreatePost;
using General.Application.Features.Posts.Commands.DeleteAttachment;
using General.Application.Features.Posts.Commands.NewAttachment;
using General.Application.Features.Posts.Queries.SearchPosts;
using General.Application.Features.Reaction.CreatePostReaction;
using General.Application.Features.Reaction.DeletePostReaction;
using General.Application.Models.Parameters.PostParameters;
using General.Application.Models.Parameters.SearchParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Implementations.Abstractions;

namespace General.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PostController : BaseController
{
    public PostController(ICommandBus commandBus, IQueryBus queryBus) : base(commandBus, queryBus)
    {
    }

    [HttpGet("[action]")] 
    public async Task<IActionResult> SearchPosts([FromQuery] SearchPostsParameters parameters)
    {
        var query = SearchPostsQuery.Create(parameters);
        var response = await QueryBus.Send(query);
        return Ok(response);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateNewPost([FromBody] CreatePostParameters parameters)
    {
        var command = CreatePostCommand.Create(parameters);
        var response = await CommandBus.Send(command);
        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreatePostReaction([FromBody] CreatePostReactionParameters parameters)
    {
        var command = CreatePostReactionCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateNewAttachment([FromForm] NewAttachmentParameters parameters)
    {
        var command = NewAttachmentCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteAttachment([FromBody] DeleteAttachmentParameters parameters)
    {
        var command = DeleteAttachmentCommand.Create(parameters);
        await CommandBus.Send(command);
        return NoContent();
    }

    [HttpDelete("[action]/{postId}")]
    public async Task<IActionResult> DeletePostReaction([FromRoute] int postId)
    {
        var command = new DeletePostReactionCommand(postId);
        await CommandBus.Send(command);
        return NoContent();
    }
}