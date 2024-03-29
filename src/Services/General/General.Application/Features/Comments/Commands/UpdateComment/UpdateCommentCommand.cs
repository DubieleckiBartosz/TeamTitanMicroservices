﻿using General.Application.Models.Parameters.CommentParameters;
using MediatR;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Comments.Commands.UpdateComment;

public record UpdateCommentCommand(int CommentId, string NewContent) : ICommand<Unit>
{
    public static UpdateCommentCommand Create(UpdateCommentParameters parameters)
    {
        return new UpdateCommentCommand(parameters.CommentId, parameters.NewContent);
    }
}