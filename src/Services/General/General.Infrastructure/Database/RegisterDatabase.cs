using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace General.Infrastructure.Database;

public static class RegisterDatabase
{
    public static IServiceCollection RegisterGeneralDatabase(this IServiceCollection services, string connection)
    {
        services.AddDbContext<GeneralContext>(options =>
        {
            options.UseSqlServer(connection);
        });

        return services;
    }

}