using General.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Base;

namespace General.Infrastructure.Database;

public class GeneralContext : DbContext
{
    public DbSet<Comment> Comments { get; set; } 
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
            .Where(e => e.Entity is Entity && e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

        foreach (var entityEntry in entries)
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    ((Entity)entityEntry.Entity).Watcher = Watcher.Create();
                    break;
                case EntityState.Modified:
                    ((Entity)entityEntry.Entity).Watcher!.Update();
                    break;
                case EntityState.Deleted:
                    entityEntry.State = EntityState.Modified;
                    ((Entity)entityEntry.Entity).Watcher!.Delete();
                    break;
            } 
        }

        return await base.SaveChangesAsync();
    }
}