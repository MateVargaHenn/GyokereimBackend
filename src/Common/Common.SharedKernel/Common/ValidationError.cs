namespace Common.SharedKernel.Common;

public sealed record ValidationError : Error
{
    public ValidationError(IReadOnlyList<Error> errors)
        : base("General.Error", ErrorType.Failure, "A validation error occurred.") => Errors = errors;

    public IReadOnlyList<Error> Errors { get; } = [];
}
