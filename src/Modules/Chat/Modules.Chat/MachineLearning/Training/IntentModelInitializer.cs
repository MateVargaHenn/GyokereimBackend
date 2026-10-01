namespace Modules.Chat.MachineLearning.Training;

internal sealed class IntentModelInitializer(
    IServiceScopeFactory scopeFactory,
    IHostEnvironment environment,
    ILogger<IntentModelInitializer> logger)
    : IHostedService
{
    public async Task StartAsync(
        CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope =
            scopeFactory.CreateAsyncScope();

        ChatDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<ChatDbContext>();

        // 1. Database schema
        await dbContext.Database.MigrateAsync(cancellationToken);

        // 2. Seed
        ChatDataSeeder seeder =
            scope.ServiceProvider.GetRequiredService<ChatDataSeeder>();

        await seeder.SeedAsync(
            dbContext,
        cancellationToken);

        // 2. Modell útvonala
        string modelPath = Path.Combine(
            environment.ContentRootPath,
            "Models",
            ChatModel.FileName);

        // 3. Ha már létezik, nincs szükség újratanításra
        if (File.Exists(modelPath))
        {
            logger.LogInformation(
                "Chat intent model already exists.");

            return;
        }

        logger.LogInformation(
            "Chat intent model not found. Training model...");

        IIntentModelTrainer trainer =
            scope.ServiceProvider
                .GetRequiredService<IIntentModelTrainer>();

        await trainer.TrainAsync(
            modelPath,
            cancellationToken);

        logger.LogInformation(
            "Chat intent model successfully created.");
    }

    public Task StopAsync(
        CancellationToken cancellationToken)
        => Task.CompletedTask;
}
