using General.Domain.Entities;

namespace General.Application.Contracts;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task<Comment?> GetCommentWithReactions(int commentId);
}