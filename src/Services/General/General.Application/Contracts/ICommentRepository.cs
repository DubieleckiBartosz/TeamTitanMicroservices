using General.Application.Models.Wrappers;
using General.Domain.Entities;

namespace General.Application.Contracts;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task<Comment?> GetCommentWithReactionsAsync(int commentId);
    Task<ListWrapper<Comment>?> SearchCommentsWithReactionsAsync(int postId, int pageNumber, int pageSize);
}