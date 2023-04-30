using AutoMapper;
using General.Application.Constants;
using General.Application.Contracts;
using General.Application.Models.ViewModels;
using Shared.Implementations.Abstractions;
using Shared.Implementations.Core.Exceptions;

namespace General.Application.Features.Comments.Queries.SearchComments;

public class SearchCommentsHandler : IQueryHandler<SearchCommentsQuery, SearchViewModel<CommentViewModel>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public SearchCommentsHandler(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<SearchViewModel<CommentViewModel>> Handle(SearchCommentsQuery request, CancellationToken cancellationToken)
    {
        var searchResult = await _commentRepository.SearchCommentsWithReactions(request.PostId, request.Search.PageNumber,
            request.Search.PageSize);

        if (searchResult == null || !searchResult.Items.Any())
        {
            throw new NotFoundException(ExceptionDetails.DetailsNotFound("SearchCommentsWithReactions"),
                ExceptionTitles.TitleNotFound("SearchResult"));
        }

        var response = _mapper.Map<SearchViewModel<CommentViewModel>>(searchResult);

        return response;
    }
}