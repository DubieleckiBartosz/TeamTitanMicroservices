using Microsoft.EntityFrameworkCore;

namespace General.Infrastructure.Database;

public class AutomaticMigration
{
    private readonly GeneralContext _context;

    public AutomaticMigration(GeneralContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void RunMigration()
    {
        if (_context.Database.CanConnect())
        {
            if (_context.Database.IsRelational())
            {
                var pendingMigrations = _context.Database?.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    _context.Database?.Migrate();
                }
            } 
        }
    }
}