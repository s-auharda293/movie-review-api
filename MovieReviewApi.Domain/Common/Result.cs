using MovieReviewApi.Domain.Common;

public class Result<T>
{
    private Result(bool isSuccess, T? value, List<Error> errors)
    {
        if (isSuccess && errors.Count > 0)
            throw new ArgumentException("A successful result cannot have errors.");
        if (!isSuccess && errors.Count == 0)
            throw new ArgumentException("A failed result must have at least one error.");

        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public List<Error> Errors { get; }

    // Factory for success
    public static Result<T> Success(T value) => new Result<T>(true, value, new List<Error>());

    // Factory for failure with a single error
    public static Result<T> Failure(Error error) => new Result<T>(false, default, new List<Error> { error });

    // Factory for failure with multiple errors
    public static Result<T> Failure(List<Error> errors) => new Result<T>(false, default, errors);
}
