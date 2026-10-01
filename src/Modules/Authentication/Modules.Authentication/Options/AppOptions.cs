namespace Modules.Authentication.Options;

internal sealed class AppOptions
{
    public AppUserOptions SuperAdmin { get; init; } = new();
    public AppUserOptions Admin { get; init; } = new();
}
