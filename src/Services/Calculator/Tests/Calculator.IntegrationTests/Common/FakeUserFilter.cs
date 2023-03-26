using Calculator.IntegrationTests.Setup;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Calculator.IntegrationTests.Common;

public class FakeUserFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.User = UserSetup.UserPrincipals();

        await next();
    }
} 