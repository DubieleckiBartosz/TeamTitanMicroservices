using General.Domain.Entities;
using General.Infrastructure.Database;

namespace General.Infrastructure.Repositories;

public class CommentRepository : BaseRepository<Comment>
{
    public CommentRepository(GeneralContext context) : base(context)
    {
    }
}