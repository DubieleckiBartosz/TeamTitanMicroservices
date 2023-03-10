using Microsoft.AspNetCore.Builder;
using Shared.Implementations;

namespace Management.Application;

public static class Configurations
{
    public static WebApplication SubscribeEvents(this WebApplication app)
    {
        app.UseSubscribeAllEvents(typeof(AssemblyManagementApplicationReference).Assembly);

        return app;
    }
}