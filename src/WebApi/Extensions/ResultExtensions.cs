using Domain.Shared;

namespace WebApi.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return Results.Problem(
                statusCode: GetStatusCode(result.Error),
                title: GetTitle(result.Error),
                type: GetType(result.Error),
                extensions: new Dictionary<string, object?>
                {
                    { "errors", new[] { result.Error} }
                });

            static int GetStatusCode(Error error) =>
                error.ErrorType switch
                {
                    ErrorType.Validation or ErrorType.Problem => StatusCodes.Status400BadRequest,
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.Conflict => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError
                };

            static string GetTitle(Error error) =>
                error.ErrorType switch
                {
                    ErrorType.Validation => "Bad Request",
                    ErrorType.NotFound => "Not Found",
                    ErrorType.Conflict => "Conflict",
                    ErrorType.Problem => "Problem",
                    _ => "Server Failure"
                };

            static string GetType(Error error) =>
                error.ErrorType switch
                {
                    ErrorType.Validation => "https://tools.ietf.org/doc/html/rfc7231#section-6.5.1",
                    ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    ErrorType.NotFound => "https://tools.ietf.org/doc/html/rfc7231#section-6.5.4",
                    ErrorType.Conflict => "https://tools.ietf.org/doc/html/rfc7231#section-6.5.8",
                    _ => "https://tools.ietf.org/doc/html/rfc7231#section-6.6.1"
                };
        }
    }
}
