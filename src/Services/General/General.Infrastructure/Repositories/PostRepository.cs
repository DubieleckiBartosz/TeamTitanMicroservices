using General.Application.Contracts;
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

    public async Task<Post?> GetPostWithAttachments(int postId)
    {
        var result = await DbSet.IncludePaths(Common.NavigationAttachmentsList)
            .FirstOrDefaultAsync(_ => _.Id == postId);
        return result;
    }

    public async Task<Post?> GetPostWithReactions(int postId)
    {
        var result = await DbSet.Include(_ => _.Reactions)
            .FirstOrDefaultAsync(_ => _.Id == postId);
        return result;
    }

    public async Task<Post?> GetPostWithDetailsAsync(int postId)
    {
        throw new NotImplementedException();
    }
}