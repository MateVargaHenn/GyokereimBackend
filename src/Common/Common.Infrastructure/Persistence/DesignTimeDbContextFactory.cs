namespace Common.Infrastructure.Persistence;

public abstract class DesignTimeDbContextFactory<TContext>
    : IDesignTimeDbContextFactory<TContext>
    where TContext : DbContext
{
    protected abstract string Schema { get; }

    protected abstract TContext CreateContext(
        DbContextOptions<TContext> options, IDomainEventsDispatcher? domainEventsDispatcher);

    public TContext CreateDbContext(string[] args)
    {
        string connectionString =
            Environment.GetEnvironmentVariable(
                "ConnectionStrings__CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "ConnectionStrings__CONNECTION_STRING is not configured.");

        var optionsBuilder =
            new DbContextOptionsBuilder<TContext>();

        optionsBuilder.UseNpgsql(
            connectionString,
            options =>
                options.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    Schema));

        return CreateContext(optionsBuilder.Options, null);
    }
}
