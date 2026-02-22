namespace CoursesManager.Application.Common.Errors;


// Istället för att kasta ett exception och krascha returneras antingen 
// ett värde om det gick bra, eller ett fel om något gick fel.
public class ErrorOr<T>
{
    private readonly T? _value;
    private readonly List<Error> _errors;

    public bool IsError { get; }
    public bool IsSuccess => !IsError;

    private ErrorOr(T value)
    {
        _value = value;
        _errors = [];
        IsError = false;
    }

    private ErrorOr(List<Error> errors)
    {
        _value = default;
        _errors = errors;
        IsError = true;
    }

    public static implicit operator ErrorOr<T>(T value) => new(value);
    public static implicit operator ErrorOr<T>(Error error) => new([error]);
    public static implicit operator ErrorOr<T>(List<Error> errors) => new(errors);

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access Value when IsError is true");

    public Error FirstError => IsError && _errors.Count > 0
        ? _errors[0]
        : throw new InvalidOperationException("Cannot access FirstError when IsSuccess is true");

    public IReadOnlyList<Error> Errors => IsError
        ? _errors.AsReadOnly()
        : throw new InvalidOperationException("Cannot access Errors when IsSuccess is true");


    public TResult Match<TResult>(
        Func<T, TResult> onValue,
        Func<List<Error>, TResult> onErrors)
    {
        return IsSuccess ? onValue(_value!) : onErrors(_errors);
    }
}