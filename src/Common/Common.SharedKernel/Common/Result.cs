namespace Common.SharedKernel.Common;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if (error is null)
        {
            throw new ArgumentNullException(nameof(error), "Error cannot be null.");
        }
        if (isSuccess && error.Type != ErrorType.None)
        {
            throw new ArgumentException("Successful result cannot have an error.", nameof(error));
        }

        if (!isSuccess && error.Type == ErrorType.None)
        {
            throw new ArgumentException("Failed result must have a valid error.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }
    public Result(bool isSuccess, ValidationError error)
    {
        if (isSuccess && error != null ||
            !isSuccess && error == null)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        ValidationError = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public ValidationError? ValidationError { get; }

    public Error Error => IsSuccess
        ? null!
        : field!;

    public static Result Success() => new(true, new Error(null!, ErrorType.None, string.Empty));

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, new Error(null!, ErrorType.None, string.Empty));

    public static Result Failure(ErrorType error) => new(false, new Error(error.ToString(), error, string.Empty));

    public static Result<TValue> Failure<TValue>(Error error) =>
        new(default!, false, error);

    public static Result Failure(ValidationError error) =>
        new(false, error);
}

public class Result<TValue> : Result
{
    public Result(TValue value,
                  bool isSuccess,
                  Error error)
        : base(isSuccess, error) => Value = value;


    public TValue? Value => IsSuccess
        ? field!
        : default;

    public Error[] Errors { get; set; } = [];

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(new Error(null!, ErrorType.NullValue, string.Empty));

    public Result<TValue> ValidationFailure(string obj, ErrorType error) =>
        new(default!, false, new Error(obj, error, string.Empty));
}
