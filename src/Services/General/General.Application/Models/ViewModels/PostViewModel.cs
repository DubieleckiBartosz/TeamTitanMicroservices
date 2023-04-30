namespace General.Application.Models.ViewModels;

public class PostViewModel
{
    public int CreatedBy { get; init; }
    public bool IsPublic { get; init; }
    public string? Organization { get; init; }
    public string? Description { get; init; }
    public List<AttachmentViewModel>? Attachments { get; init; }
    public List<ReactionViewModel>? Reactions { get; init; } 
}