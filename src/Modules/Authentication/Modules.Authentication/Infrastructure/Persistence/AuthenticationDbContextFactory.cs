using Microsoft.EntityFrameworkCore.Design;

namespace Modules.WebSetting.Infrastructure.Persistence;

internal sealed class AuthenticationDbContextFactory
    : IDesignTimeDbContextFactory<AuthenticationDbContext>
{
    public AuthenticationDbContext CreateDbContext(string[] args)
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__CONNECTION_STRING")
            ?? Environment.GetEnvironmentVariable("CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "A CONNECTION_STRING környezeti változó nincs beállítva.");

        var optionsBuilder =
            new DbContextOptionsBuilder<AuthenticationDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new AuthenticationDbContext(optionsBuilder.Options);
    }
}
