namespace CoursesManager.Application.Common.Results;


public readonly record struct Deleted;
public readonly record struct Created;


public static class Result
{
    public static Deleted Deleted => default;
    public static Created Created => default;
}
