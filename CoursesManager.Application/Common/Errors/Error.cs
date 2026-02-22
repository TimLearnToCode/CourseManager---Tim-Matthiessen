namespace CoursesManager.Application.Common.Errors;

public record Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType ErrorType { get; }

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        ErrorType = type;
    }

    public static Error Validation(string code, string message) =>
        new(code, message, ErrorType.Validation);

    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);
}