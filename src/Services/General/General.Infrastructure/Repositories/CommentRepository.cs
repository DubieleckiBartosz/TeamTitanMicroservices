using General.Application.Contracts;
using General.Application.Models.Wrappers;
using General.Domain.Entities;
using General.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace General.Infrastructure.Repositories;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(GeneralContext context) : base(context)
    {
    }

    public async Task<Comment?> GetCommentWithReactions(int commentId)
    {
        var result = await DbSet.Include(_ => _.Reactions)
            .FirstOrDefaultAsync(_ => _.Id == commentId);
        return result;
    }

    public async Task<ListWrapper<Comment>?> SearchCommentsWithReactions(int postId, int pageNumber, int pageSize)
    {
        var query =  DbSet
            .Include(_ => _.Reactions)
            .Where(_ => EF.Property<int>(_, "PostId") == postId)
            .OrderByDescending(_ => _.Watcher!.Created).AsQueryable();

        var count = query.Count();

        var result = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync();


        return ListWrapper<Comment>.Wrap(result, count, pageNumber, pageSize);
    }
}