using System.Diagnostics;
using Hoteling.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Hoteling.API.Exceptions;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        var (statusCode, errorCode, message) = MapException(exception);

        if (statusCode >= 500)
        {
            logger.LogError(exception, "Internal server ERROR [{TraceId}]: {Message}", traceId, exception.Message);
        }
        else
        {
            logger.LogWarning("Bad Request: [{TraceId}]: {Code} - {Message}", traceId, errorCode, message);
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = errorCode,
            Detail = message,
            Instance = httpContext.Request.Path,
            Extensions = { ["traceId"] = traceId }
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static (int StatusCode, string ErrorCode, string Message) MapException(Exception exception)
    {
        return exception switch
        {
            AppException ex => ((int)ex.StatusCode, ex.ErrorCode, ex.Message),
            OperationCanceledException => (499, "CANCELLED", "Request was cancelled"),
            UnauthorizedAccessException => (401, "UNAUTHORIZED", "Unauthorized access"),
            _ => (500, "INTERNAL_SERVER_ERROR", "An unexpected error occurred.")
        };
    }
}
