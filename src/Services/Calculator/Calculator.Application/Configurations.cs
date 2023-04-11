using Microsoft.AspNetCore.Builder;
using Shared.Implementations;

namespace Calculator.Application;

public static class Configurations
{
    public static WebApplication SubscribeEvents(this WebApplication app)
    {
        app.UseSubscribeAllEvents(typeof(AssemblyCalculatorApplicationReference).Assembly);

        return app;
    }
}