using General.Application.Models.Wrappers;
using General.Domain.Entities;

namespace General.Application.Contracts;

public interface IPostRepository : IBaseRepository<Post>
{
    Task<Post?> GetPostWithCommentsAsync(int postId);
    Task<Post?> GetPostWithAttachmentsAsync(int postId);
    Task<Post?> GetPostWithReactionsAsync(int postId);
    Task<ListWrapper<Post>?> SearchPostsAsync(int pageSize, int pageNumber);
}