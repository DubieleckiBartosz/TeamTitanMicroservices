using General.Application.Models.Wrappers;
using General.Domain.Entities;

namespace General.Application.Contracts;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task<Comment?> GetCommentWithReactions(int commentId);
    Task<ListWrapper<Comment>?> SearchCommentsWithReactions(int postId, int pageNumber, int pageSize);
}