using Microsoft.EntityFrameworkCore;
using Shared.Implementations.Logging;

namespace General.Infrastructure.Database;

public class AutomaticMigration
{
    private readonly GeneralContext _context;
    private readonly ILoggerManager<AutomaticMigration> _loggerManager;

    public AutomaticMigration(GeneralContext context, ILoggerManager<AutomaticMigration> loggerManager)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
    }

    public void RunMigration()
    {
        try
        {
            if (_context.Database.CanConnect())
            {
                if (_context.Database.IsRelational())
                {
                    var pendingMigrations = _context.Database?.GetPendingMigrations()?.ToList();
                    if (pendingMigrations != null && pendingMigrations.Any())
                    {
                        _loggerManager.LogInformation($"Before migrations : {string.Join(", ", pendingMigrations)} - {DateTime.UtcNow}");
                        _context.Database?.Migrate();
                        _loggerManager.LogInformation($"After migrations - {DateTime.UtcNow}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(new
            {
                Message = ex.Message,
                MigrationException = ex.InnerException,
            });
            throw;
        }
    }
}