using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shared.Implementations.Core.Exceptions;
using Shared.Implementations.Logging;

namespace Shared.Implementations.Core;

public class ErrorHandlingMiddleware
{
    private const string ServerError = "Server Error";

    private readonly RequestDelegate _next;
    private readonly Func<Exception, int> _funcStatusCode;
    private readonly Func<Exception, int, object> _funcResponse;

    public ErrorHandlingMiddleware(RequestDelegate next, Func<Exception, int> funcStatusCode,
        Func<Exception, int, object> funcResponse)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _funcStatusCode = funcStatusCode ?? throw new ArgumentNullException(nameof(funcStatusCode));
        _funcResponse = funcResponse ?? throw new ArgumentNullException(nameof(funcResponse));
    }

    public async Task Invoke(HttpContext context, ILoggerManager<ErrorHandlingMiddleware> loggerManager)
    {
        try
        {
            await this._next(context);
        }
        catch (Exception e)
        {
            loggerManager.LogError(e?.Message);
            await HandleExceptionAsync(context, e);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = _funcStatusCode.Invoke(exception);
        if (statusCode == 0)
        {
            statusCode = GetStatusCode(exception);
        }


        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = statusCode;

        var response = _funcResponse.Invoke(exception, statusCode);

        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            ArgumentOutOfRangeException => StatusCodes.Status400BadRequest,
            ArgumentNullException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            ValidatorException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            NotFoundException e => (int)e.StatusCode,
            BadRequestException e => (int)e.StatusCode,
            DatabaseException e => (int)e.StatusCode,
            EventException e => (int)e.StatusCode,
            TeamTitanApplicationException e => (int)e.StatusCode,
            _ => StatusCodes.Status500InternalServerError
        };

    public static string GetGlobalTitle(Exception exception) =>
        exception switch
        {
            NotFoundException notFoundException => notFoundException.Title,
            BadRequestException badRequestException => badRequestException.Title,
            DatabaseException dbException => dbException.Title,
            EventException eventException => eventException.Title,
            TeamTitanApplicationException teamTitanApplicationException => teamTitanApplicationException.Title,
            _ => ServerError
        };

    public static IReadOnlyList<string>? AssignErrors(Exception exception)
    {
        IReadOnlyList<string>? errors = null;

        if (exception is ValidatorException validatorException)
        {
            errors = validatorException.Errors;
        }

        return errors;
    }

    public static object GetBaseErrorResponse(Exception exception, int statusCode)
    {
        var response = new
        {
            title = ErrorHandlingMiddleware.GetGlobalTitle(exception),
            status = statusCode,
            detail = exception.Message,
            errors = ErrorHandlingMiddleware.AssignErrors(exception)
        };

        return response;
    }
}

public static class ExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder,
        Func<Exception, int> funcStatusCode, Func<Exception, int, object> funcResponse)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>(funcStatusCode, funcResponse);
    }
}