namespace Modules.Service.Infrastructure.Persistence;

internal sealed class ServiceDbContextFactory
    : IDesignTimeDbContextFactory<ServiceDbContext>
{
    public ServiceDbContext CreateDbContext(string[] args)
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__CONNECTION_STRING")
            ?? Environment.GetEnvironmentVariable("CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "A CONNECTION_STRING környezeti változó nincs beállítva.");

        var optionsBuilder =
            new DbContextOptionsBuilder<ServiceDbContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new ServiceDbContext(optionsBuilder.Options, null);
    }
}
