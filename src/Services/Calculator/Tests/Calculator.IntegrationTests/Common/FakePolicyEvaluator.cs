using Calculator.IntegrationTests.Setup;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;

namespace Calculator.IntegrationTests.Common;

public class FakePolicyEvaluator : IPolicyEvaluator
{
    public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        var claimsPrincipal = UserSetup.UserPrincipals();

        var authTicket = new AuthenticationTicket(claimsPrincipal, "ticketTest");
        var result = AuthenticateResult.Success(authTicket);
        return Task.FromResult(result);
    }

    public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
        AuthenticateResult authenticationResult, HttpContext context,
        object? resource)
    {
        var result = PolicyAuthorizationResult.Success();

        return Task.FromResult(result);
    }
}