using AutoMapper;
using General.Application.Constants;
using General.Application.Contracts;
using General.Application.Models.ViewModels;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Services;

namespace General.Application.Features.Posts.Queries.SearchPosts;

public class SearchPostsHandler : IQueryHandler<SearchPostsQuery, SearchViewModel<PostViewModel>>
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public SearchPostsHandler(IPostRepository postRepository, IMapper mapper, ICurrentUser currentUser)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
    public async Task<SearchViewModel<PostViewModel>> Handle(SearchPostsQuery request, CancellationToken cancellationToken)
    {
        var organizationCode = _currentUser.OrganizationCode;
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