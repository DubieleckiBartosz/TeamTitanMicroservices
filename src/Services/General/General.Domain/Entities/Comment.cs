using General.Domain.ValueObjects;
using Shared.Domain.Base;
using Shared.Domain.DomainExceptions;

namespace General.Domain.Entities;

public class Comment : Entity
{
    public int Creator { get; private set; } 
    public Content Content { get; private set; }
    public List<Reaction> Reactions { get; private set; } = new();

    private Comment()
    { 
    }

    private Comment(int creator, Content content)
    {
        Creator = creator;
        Content = content; 
    }

    public static Comment Create(int creator, string content) => new Comment(creator, Content.Create(content)); 

    public void AddNewReaction(int creator, int type)
    {
        
        var reactionExists= Reactions.Any(_ => _.Creator == creator);
        if (reactionExists)
        {
            throw new BusinessException("Double reaction", "Only one user reaction per comment");
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
            throw new BusinessException("Reaction not found", $"No reaction found in comment {Id}");
        }

        Reactions.Remove(reaction);
        IncrementVersion();
    }
}