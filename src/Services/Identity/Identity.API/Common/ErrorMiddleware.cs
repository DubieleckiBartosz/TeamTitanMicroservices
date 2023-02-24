using Identity.Application.Common.Exceptions;
using Shared.Implementations.Core;

namespace Identity.API.Common;

public class ErrorMiddleware
{
    public static object GetErrorResponse(Exception exception, int statusCode)
    {
        var title = GetTitle(exception);
        var response = new
        {
            title = string.IsNullOrEmpty(title) ? ErrorHandlingMiddleware.GetGlobalTitle(exception) : title,
            status = statusCode,
            detail = exception.Message,
            errors = GetErrors(exception)
        };

        return response;
    }

    public static int GetStatusCode(Exception exception)
    {
        var statusCode = exception switch
        { 
            AuthException authException => authException.Code,
            IdentityResultException identityResultException => (int)identityResultException.StatusCode,
            _ => 0
        };

        return statusCode;
    }

    private static string GetTitle(Exception exception) =>
        exception switch
        { 
            AuthException authException => authException.Title,
            IdentityResultException identityResultException => identityResultException.Title,
            _ => string.Empty
        };

    private static IReadOnlyList<string>? GetErrors(Exception exception)
    {
        IReadOnlyList<string>? errors = null;

        if (exception is IdentityResultException validationException)
        {
            errors = validationException.Errors;
        }

        return errors;
    }
}