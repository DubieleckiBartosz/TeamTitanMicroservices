using General.Application.Constants;
using General.Application.Contracts;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Features.Reaction.DeletePostReaction;

public class DeletePostReactionHandler : ICommandHandler<DeletePostReactionCommand, Unit>
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUser _currentUser;

    public DeletePostReactionHandler(IPostRepository postRepository, ICurrentUser currentUser)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<Unit> Handle(DeletePostReactionCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetPostWithReactionsAsync(request.PostId);
        if (post == null)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("GetPostWithReactions"),
                ExceptionTitles.TitleNotFound("Post"));
        }

        post.RemoveReaction(_currentUser.UserId);
        _postRepository.Update(post);
        await _postRepository.SaveAsync();

        return Unit.Value;
    }
}