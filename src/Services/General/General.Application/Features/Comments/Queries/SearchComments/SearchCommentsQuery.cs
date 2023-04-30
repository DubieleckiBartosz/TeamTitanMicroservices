using General.Application.Models.Parameters.SearchParameters;
using General.Application.Models.ViewModels;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Search;

namespace General.Application.Features.Comments.Queries.SearchComments;

public record SearchCommentsQuery(int PostId, BaseSearchQuery Search) : IQuery<SearchViewModel<CommentViewModel>>
{
    public static SearchCommentsQuery Create(SearchCommentsParameters parameters)
    {
        return new SearchCommentsQuery(parameters.PostId,
            new BaseSearchQuery(parameters.PageNumber, parameters.PageSize));
    }
}