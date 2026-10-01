namespace Modules.File;

internal sealed class FileInitializer(
    IServiceScopeFactory scopeFactory)
    : IHostedService
{
    public async Task StartAsync(
        CancellationToken cancellationToken)
    {
        using IServiceScope scope =
            scopeFactory.CreateScope();

        FileDbContext dbContext =
            scope.ServiceProvider
                .GetRequiredService<FileDbContext>();

        await dbContext.Database.MigrateAsync(
            cancellationToken);
    }

    public Task StopAsync(
        CancellationToken cancellationToken)
        => Task.CompletedTask;
}
