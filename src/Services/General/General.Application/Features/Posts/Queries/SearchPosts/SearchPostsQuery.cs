using General.Application.Models.Parameters.SearchParameters;
using General.Application.Models.ViewModels;
using Shared.Implementations.Abstractions;

namespace General.Application.Features.Posts.Queries.SearchPosts;

public record SearchPostsQuery(int PageNumber, int PageSize) : IQuery<SearchViewModel<PostViewModel>>
{
    public static SearchPostsQuery Create(SearchPostsParameters parameters)
    {
        return new SearchPostsQuery(parameters.PageNumber, parameters.PageSize);
    }
}