using General.Application.Contracts;
using General.Application.Models.Wrappers;
using General.Domain.Entities;
using General.Infrastructure.Database;
using General.Infrastructure.Database.Extensions;
using Microsoft.EntityFrameworkCore;

namespace General.Infrastructure.Repositories;

public class PostRepository : BaseRepository<Post>, IPostRepository
{ 
    public PostRepository(GeneralContext context) : base(context)
    { 
    }

    public async Task<Post?> GetPostWithCommentsAsync(int postId)
    {
        var result = await this.DbSet
            .Include(_ => _.Comments)
            .FirstOrDefaultAsync(_ => _.Id == postId);

        return result;
    }

    public async Task<Post?> GetPostWithAttachmentsAsync(int postId)
    {
        var result = await DbSet.IncludePaths(Common.NavigationAttachmentsList)
            .FirstOrDefaultAsync(_ => _.Id == postId);
        return result;
    }

    public async Task<Post?> GetPostWithReactionsAsync(int postId)
    {
        var result = await DbSet.Include(_ => _.Reactions)
            .FirstOrDefaultAsync(_ => _.Id == postId);
        return result;
    }
    
    public async Task<ListWrapper<Post>?> SearchPostsAsync(int pageSize, int pageNumber, string? organizationKey = null)
    {
        var query = DbSet.Include(_ => _.Reactions)
            .IncludePaths(Common.NavigationAttachmentsList)
            .Where(_ => organizationKey == null ? _.IsPublic == true : _.IsPublic || _.Organization == organizationKey)
            .OrderByDescending(_ => _.CreatedBy).AsQueryable();

        var count = query.Count();
        var result = await query.Skip(pageNumber - 1).Take(pageSize).ToListAsync();

        return ListWrapper<Post>.Wrap(result, count, pageNumber, pageSize);
    }

}