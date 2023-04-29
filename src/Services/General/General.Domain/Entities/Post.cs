﻿using General.Domain.ValueObjects;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;

namespace General.Domain.Entities;

public class Post : Entity, IAggregateRoot
{
    public int CreatedBy { get; private init; }  
    public bool IsPublic { get; private set; }
    public string? Organization { get; private set; }
    public Content? Content { get; private set; }
    public List<Comment> Comments { get; private set; }  
    public List<Reaction> Reactions { get; private set; }  
    public List<Attachment> Attachments { get; private set; }  

    private Post()
    {
        Reactions = new();
        Attachments = new List<Attachment>();
        Comments = new List<Comment>();
    }

    private Post(int creator, string? description, bool isPublic, string? organization, Attachment? attachment)
    {
        CreatedBy = creator;
        Organization = organization;
        IsPublic = isPublic;
        if (description != null)
        {
            Content = Content.Create(description);
        }

        if (attachment != null)
        { 
            Attachments = new() {attachment};
        }
    }

    public static Post Create(int creator, string? description, bool isPublic, string organization,
        Attachment? attachment)
    {
        return new(creator, description, isPublic, organization, attachment);
    }

    public Comment AddComment(int creator, string content)
    {
        var newComment = Comment.Create(creator, content);
        Comments.Add(newComment);
        IncrementVersion();

        return newComment;
    }

    public void AsPublic(string organization)
    {
        IsPublic = true;
        Organization = organization;
    }

    public void RemoveComment(int commentId)
    {
        var comment = GetCommentById(commentId);  
        Comments.Remove(comment);
        IncrementVersion();
    }  

    public void AddNewReaction(int creator, int type)
    {
        var reactionExists = Reactions.Any(_ => _.Creator == creator);
        if (reactionExists)
        {
            throw new BusinessException("Double reaction", "Only one user reaction per post");
        }

        var newReaction = Reaction.CreateReaction(creator, type);
        Reactions.Add(newReaction);
        IncrementVersion();
    }

    public void RemoveReaction(int creator)
    {
        var reaction = Reactions.FirstOrDefault(_ => _.Creator == creator);
        if (reaction == null)
        {
            throw new BusinessException("Reaction not found", $"No reaction found in post {Id}");
        }

        Reactions.Remove(reaction);
        IncrementVersion();
    }

    public Comment GetCommentById(int commentId)
    {
        return Comments.FirstOrDefault(_ => _.Id == commentId) ??
               throw new BusinessException("Comment not found", "The comment must exist to be removed");
    }
}