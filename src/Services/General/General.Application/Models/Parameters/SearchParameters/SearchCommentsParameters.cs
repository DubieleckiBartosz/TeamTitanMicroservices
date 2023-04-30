using Shared.Implementations.Search.SearchParameters;

namespace General.Application.Models.Parameters.SearchParameters;

public class SearchCommentsParameters : BaseSearchQueryParameters
{
    public int PostId { get; init; } 
}