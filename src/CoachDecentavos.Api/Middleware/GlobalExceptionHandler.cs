using System.Net;
using System.Text.Json;
using CoachDecentavos.Application.Common;
using CoachDecentavos.Application.Common.Exceptions;

namespace CoachDecentavos.Api.Middleware;

public sealed class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            AppException appEx => appEx.StatusCode,
            _ => (int)HttpStatusCode.InternalServerError
        };

        if (statusCode >= 500)
            _logger.LogError(exception, "Unhandled exception");
        else
            _logger.LogWarning(exception, "Request failed with {StatusCode}", statusCode);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var message = statusCode >= 500 && !_environment.IsDevelopment()
            ? "An unexpected error occurred."
            : exception.Message;

        var payload = new ApiErrorResponse(message, context.TraceIdentifier);
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}