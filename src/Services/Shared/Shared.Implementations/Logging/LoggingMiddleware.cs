using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Shared.Implementations.Logging;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILoggerManager<LoggingMiddleware> loggerManager)
    {
        this.LogRequest(context, loggerManager);

        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        await this.LogResponse(context, loggerManager);
        await responseBody.CopyToAsync(originalBodyStream);
    }

    private void LogRequest(HttpContext context, ILoggerManager<LoggingMiddleware> loggerManager)
    {
        loggerManager.LogInformation(new
        {
            LogType = "Http Request Information",
            Schema = context.Request.Scheme,
            Host = context.Request.Host,
            Path = context.Request.Path,
            Query = context.Request.QueryString
        });
    }
    private async Task LogResponse(HttpContext context, ILoggerManager<LoggingMiddleware> loggerManager)
    {
        var response = await FormatResponse(context.Response);
        loggerManager.LogInformation(new
        {
            LogType = "Http Response Information",
            Path = context.Request?.Path,
            StatusCode = context.Response?.StatusCode,
            Response = response
        });
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);

        string text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return text;
    }
}

public static class LoggingHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggerHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}
