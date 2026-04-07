using System.Diagnostics;

namespace Logging.Sample.Middleware;

public sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        await next(context);
        stopwatch.Stop();

        var message = "{Method} {Path} -> {StatusCode} in {ElapsedMs}ms";

        if (context.Response.StatusCode >= 400)
        {
            logger.LogWarning(message, context.Request.Method, context.Request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
        else
        {
            logger.LogInformation(message, context.Request.Method, context.Request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
    }
}
