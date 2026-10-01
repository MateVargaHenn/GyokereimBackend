namespace Modules.Authentication.Options;

internal sealed class AppUserOptions
{
    public string User { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
