using General.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Base;

namespace General.Infrastructure.Database;

public class GeneralContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public GeneralContext(DbContextOptions<GeneralContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GeneralContext).Assembly);
    }

    public async Task<int> SaveChangesAsync()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Entity && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {

            if (entityEntry.State == EntityState.Added)
            {
                ((Entity) entityEntry.Entity).Watcher = Watcher.Create();
            }
            else
            {
                ((Entity)entityEntry.Entity).Watcher!.Update();
            }
        }

        return await base.SaveChangesAsync();
    }
}