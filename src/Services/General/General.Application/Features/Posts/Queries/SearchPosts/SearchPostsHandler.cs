using AutoMapper;
using General.Application.Constants;
using General.Application.Contracts;
using General.Application.Models.ViewModels;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;

namespace General.Application.Features.Posts.Queries.SearchPosts;

public class SearchPostsHandler : IQueryHandler<SearchPostsQuery, SearchViewModel<PostViewModel>>
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public SearchPostsHandler(IPostRepository postRepository, IMapper mapper)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<SearchViewModel<PostViewModel>> Handle(SearchPostsQuery request, CancellationToken cancellationToken)
    {
        var searchResult = await _postRepository.SearchPostsAsync(request.PageSize, request.PageNumber);

        if (searchResult == null || !searchResult.Items.Any())
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("SearchPostsAsync"),
                ExceptionTitles.TitleNotFound("SearchResult"));
        }

        var response = _mapper.Map<SearchViewModel<PostViewModel>>(searchResult);

        return response;
    }
}