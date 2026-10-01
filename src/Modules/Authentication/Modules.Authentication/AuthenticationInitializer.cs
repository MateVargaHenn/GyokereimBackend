using Microsoft.Extensions.Logging;
using Modules.Authentication.Infrastructure.Persistence.Seed;

namespace Modules.Authentication;

internal sealed class AuthenticationInitializer(
    IServiceScopeFactory scopeFactory,
    ILogger<AuthenticationInitializer> logger)
    : IHostedService
{
    public async Task StartAsync(
        CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope =
            scopeFactory.CreateAsyncScope();

        AuthenticationDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

        // 1. Database schema
        await dbContext.Database.MigrateAsync(cancellationToken);

        // 2. Seed
        AuthenticationSeeder seeder =
            scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();

        logger.LogInformation(
            "Authentication data seeding started.");
        
        await seeder.SeedAsync();

        logger.LogInformation(
            "Authentication data successfully seeded.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
