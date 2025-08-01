using Domain.Results;

namespace Lunches.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return Results.Problem(
            statusCode: GetStatusCode(result.Error.DomainErrorType),
            title: GetTitle(result.Error.DomainErrorType),
            type: GetType(result.Error.DomainErrorType),
            detail: result.Error.Description);

        #region Mappings

        static int GetStatusCode(DomainErrorType errorType) => errorType switch
        {
            DomainErrorType.Validation => StatusCodes.Status400BadRequest,
            DomainErrorType.NotFound => StatusCodes.Status404NotFound,
            DomainErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        static string GetTitle(DomainErrorType errorType) => errorType switch
        {
            DomainErrorType.Validation => "Bad Request",
            DomainErrorType.NotFound => "Not Found",
            DomainErrorType.Conflict => "Conflict",
            _ => "Server Failure"
        };

        static string GetType(DomainErrorType errorType) => errorType switch
        {
            DomainErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            DomainErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            DomainErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        #endregion
    }
}