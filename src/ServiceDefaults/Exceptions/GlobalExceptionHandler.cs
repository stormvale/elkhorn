using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ServiceDefaults.Exceptions;

/// <summary>
/// An Exception Handler that will convert any Exceptions to a ProblemDetails which will be returned to the caller.
/// </summary>
public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        LogUnhandledException(logger, exception);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "An unhandled exception occurred",
                Detail = exception.Message
            }
        });
    }
    
    private static readonly Action<ILogger, Exception?> LogUnhandledException =
        LoggerMessage.Define(LogLevel.Error, new EventId(1000, nameof(Exception)), "Unhandled exception occurred.");
}