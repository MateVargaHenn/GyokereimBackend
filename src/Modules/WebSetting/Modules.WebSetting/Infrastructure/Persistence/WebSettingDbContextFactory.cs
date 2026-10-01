using Microsoft.EntityFrameworkCore.Design;

namespace Modules.WebSetting.Infrastructure.Persistence;

internal sealed class WebSettingDbContextFactory
    : IDesignTimeDbContextFactory<WebSettingDbContext>
{
    public WebSettingDbContext CreateDbContext(string[] args)
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__CONNECTION_STRING")
            ?? Environment.GetEnvironmentVariable("CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "A CONNECTION_STRING környezeti változó nincs beállítva.");

        var optionsBuilder =
            new DbContextOptionsBuilder<WebSettingDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new WebSettingDbContext(optionsBuilder.Options);
    }
}
