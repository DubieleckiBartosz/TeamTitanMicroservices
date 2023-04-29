﻿using General.Application.Constants;
using General.Application.Contracts;
using MediatR;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Features.Reaction.CreatePostReaction;

public class CreatePostReactionHandler : ICommandHandler<CreatePostReactionCommand, Unit>
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUser _currentUser;

    public CreatePostReactionHandler(IPostRepository postRepository, ICurrentUser currentUser)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<Unit> Handle(CreatePostReactionCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetPostWithReactions(request.PostId);
        if (post == null)
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("GetPostWithReactions"),
                ExceptionTitles.TitleNotFound("Post"));
        }

        post.AddNewReaction(_currentUser.UserId, (int) request.Reaction);
        _postRepository.Update(post);
        await _postRepository.SaveAsync();

        return Unit.Value;
    }
}