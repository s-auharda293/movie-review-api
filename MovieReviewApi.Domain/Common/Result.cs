﻿using MovieReviewApi.Domain.Common;

public class Result<T>
{
    private Result(bool isSuccess, T? value, Error error)
    {
        if (isSuccess && error != Error.None
            || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error type", nameof(error));
        }

        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public T? Value { get; }
    public Error Error { get; }

    public static Result<T> Success(T value) => new Result<T>(true, value, Error.None);
    public static Result<T> Failure(Error error) => new Result<T>(false, default, error);
}
