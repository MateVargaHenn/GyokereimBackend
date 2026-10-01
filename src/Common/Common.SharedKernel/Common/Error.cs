namespace Common.SharedKernel.Common;

public record Error
{
    public static readonly Error None = new(null!, ErrorType.None, string.Empty);
    public static readonly Error NullValue = new(
        null!,
        ErrorType.NullValue,
        string.Empty);
    public static readonly Error NotFound = new(
        null!,
        ErrorType.NotFound,
        string.Empty);

    public Error(string code, ErrorType type, string description)
    {
        Code = code?.ToString() ?? string.Empty;
        Type = type;
        Description = description ?? string.Empty;
    }

    public string Code { get; }

    public ErrorType Type { get; }
    public string Description { get; set; }

    public static Error InvalidCredentials(string code) => new(code, ErrorType.Unauthorized, "The user provided invalid credentials.");

    public static Error Failure(string code) =>
        new(code, ErrorType.Failure, "An unexpected error occurred.");

    public static Error Problem(string code) =>
        new(code, ErrorType.Problem, "A problem occurred while processing the request.");

    public static Error Conflict(string code) =>
        new(code, ErrorType.Conflict, "A conflict occurred with the current state of the resource.");
}
