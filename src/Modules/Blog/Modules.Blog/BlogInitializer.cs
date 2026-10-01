namespace Modules.Blog;

internal sealed class BlogInitializer(
    IServiceScopeFactory scopeFactory)
    : IHostedService
{
    public async Task StartAsync(
        CancellationToken cancellationToken)
    {
        using IServiceScope scope =
            scopeFactory.CreateScope();

        BlogDbContext dbContext =
            scope.ServiceProvider
                .GetRequiredService<BlogDbContext>();

        await dbContext.Database.MigrateAsync(
            cancellationToken);
    }

    public Task StopAsync(
        CancellationToken cancellationToken)
        => Task.CompletedTask;
}
