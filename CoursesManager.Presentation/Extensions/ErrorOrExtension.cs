using CoursesManager.Application.Common.Errors;
namespace CoursesManager.Presentation.Extensions;

public static class ErrorOrExtension
{
    public static IResult ToProblemDetails(this List<Error> errors)
    {
        if (errors[0].ErrorType == ErrorType.Validation)
            return ToValidationProblem(errors);

        return ToProblem(errors[0]);
    }

    private static IResult ToValidationProblem(List<Error> errors)
    {
        var errorDictionary = errors.GroupBy(x => x.Code).ToDictionary(x => x.Key, x => x.Select(x => x.Message).ToArray());
        return Results.ValidationProblem(errorDictionary);
    }

    private static IResult ToProblem(Error error)
    {
        var statusCode = MapErrorTypeToStatusCode(error.ErrorType);
        return Results.Problem(
            statusCode : statusCode,
            detail: error.Message
        );
    }

    private static int MapErrorTypeToStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
}