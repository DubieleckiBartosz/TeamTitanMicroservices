using General.Application.Contracts;
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
}