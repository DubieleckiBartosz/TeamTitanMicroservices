using General.Application.Contracts;
using General.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace General.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity>: IBaseRepository<TEntity> where TEntity : class
{
    protected GeneralContext Context;

    protected readonly DbSet<TEntity> DbSet;

    protected BaseRepository(GeneralContext context)
    {
        this.Context = context;
        this.DbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await Context.SaveChangesAsync();
    }
}