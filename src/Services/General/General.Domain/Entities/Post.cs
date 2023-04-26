using General.Domain.ValueObjects;
using Shared.Domain.Abstractions;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;

namespace General.Domain.Entities;

public class Post : Entity, IAggregateRoot
{
    public string CreatedBy { get; private init; } = default!;
    public Content? Content { get; private set; }
    public List<Comment> Comments { get; private set; }  
    public List<Reaction> Reactions { get; private set; }  
    public List<Attachment> Attachments { get; private set; }  

    private Post()
    {
        Reactions = new();
        Attachments = new();
        Comments = new();
    }

    private Post(string creator, string? description, Attachment? attachment)
    {
        CreatedBy = creator;
        if (description != null)
        {
            Content = Content.Create(description);
        }

        if (attachment != null)
        { 
            Attachments = new() {attachment};
        }
    }

    public static Post Create(string creator, string? description, Attachment? attachment) =>
        new Post(creator, description, attachment);

    public void AddComment(int creator, string content)
    {
        var commentContent = Content.Create(content);
        var newComment = Comment.Create(creator, content);
    }

    public void RemoveComment()
    {

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
    }

    public void RemoveReaction(int creator)
    {
        var reaction = Reactions.FirstOrDefault(_ => _.Creator == creator);
        if (reaction == null)
        {
            throw new BusinessException("Reaction not found", $"No reaction found in post {Id}");
        }

        Reactions.Remove(reaction);
    }
}