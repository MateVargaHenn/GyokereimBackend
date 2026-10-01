namespace Modules.Service;

internal sealed class ServiceInitializer(
    IServiceScopeFactory scopeFactory,
    ILogger<ServiceInitializer> logger)
    : IHostedService
{
    public async Task StartAsync(
        CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope =
            scopeFactory.CreateAsyncScope();

        ServiceDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<ServiceDbContext>();

        // 1. Database schema
        await dbContext.Database.MigrateAsync(cancellationToken);

        // 2. Seed
        ServiceDataSeeder serviceDataSeeder =
            scope.ServiceProvider.GetRequiredService<ServiceDataSeeder>();

        OrderStatusSeeder orderStatusSeeder =
            scope.ServiceProvider.GetRequiredService<OrderStatusSeeder>();

        logger.LogInformation(
            "Service data seeding started.");
        
        await orderStatusSeeder.SeedDataAsync();

        await serviceDataSeeder.SeedAsync(
            dbContext,
            cancellationToken);

        logger.LogInformation(
            "Service data successfully seeded.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
