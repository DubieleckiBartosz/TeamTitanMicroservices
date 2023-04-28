using General.Domain.Entities;
using General.Infrastructure.Database;

namespace General.Infrastructure.Repositories;

public class PostRepository : BaseRepository<Post>
{ 
    public PostRepository(GeneralContext context) : base(context)
    { 
    } 
}