using System.Text.Json;
using ErrorHandling.Sample.Exceptions;

namespace ErrorHandling.Sample.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception for {Path}", context.Request.Path);
            await WriteErrorAsync(context, exception);
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, Exception exception)
    {
        var (statusCode, type, detail) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "not_found", exception.Message),
            ConflictException => (StatusCodes.Status409Conflict, "conflict", exception.Message),
            BusinessRuleException => (StatusCodes.Status422UnprocessableEntity, "business_rule", exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "unexpected_error", "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = JsonSerializer.Serialize(new
        {
            type,
            detail,
            traceId = context.TraceIdentifier
        });

        await context.Response.WriteAsync(payload);
    }
}
