

namespace Modules.WebSetting;

internal sealed class WebSettingInitializer(
    IServiceScopeFactory scopeFactory,
    ILogger<WebSettingInitializer> logger)
    : IHostedService
{
    public async Task StartAsync(
        CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope =
            scopeFactory.CreateAsyncScope();

        WebSettingDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<WebSettingDbContext>();

        // 1. Database schema
        await dbContext.Database.MigrateAsync(cancellationToken);

        // 2. Seed
        WebSettingDataSeeder seeder =
            scope.ServiceProvider.GetRequiredService<WebSettingDataSeeder>();

        logger.LogInformation(
            "Web setting data seeding started.");
        await seeder.SeedAsync(
            dbContext,
            cancellationToken);

        logger.LogInformation(
            "Web setting data successfully seeded.");
    }

    public Task StopAsync(CancellationToken cancellationToken) 
        => Task.CompletedTask;
}
