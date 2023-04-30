namespace General.Application.Models.ViewModels;

public class CommentViewModel
{
    public int Creator { get; init; }
    public string CreatorName { get; init; }
    public string Description { get; init; } = default!;
    public List<ReactionViewModel> Reactions { get; init; } = new();
}