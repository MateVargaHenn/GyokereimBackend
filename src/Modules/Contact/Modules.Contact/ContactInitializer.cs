namespace Modules.Contact;

internal sealed class ContactInitializer(
    IServiceScopeFactory scopeFactory)
    : IHostedService
{
    public async Task StartAsync(
        CancellationToken cancellationToken)
    {
        using IServiceScope scope =
            scopeFactory.CreateScope();

        ContactDbContext dbContext =
            scope.ServiceProvider
                .GetRequiredService<ContactDbContext>();

        await dbContext.Database.MigrateAsync(
            cancellationToken);
    }

    public Task StopAsync(
        CancellationToken cancellationToken)
        => Task.CompletedTask;
}
