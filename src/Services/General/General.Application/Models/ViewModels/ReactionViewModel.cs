 

using General.Application.Models.Enums;

namespace General.Application.Models.ViewModels;

public class ReactionViewModel
{
    public int Creator { get; init; }
    public string CreatorName { get; init; } 
    public ReactionType Reaction { get; init; }
}