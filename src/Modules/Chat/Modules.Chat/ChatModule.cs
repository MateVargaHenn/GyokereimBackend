using Modules.Chat.Application;
using Modules.Chat.Infrastructure.Providers;

namespace Modules.Chat;

public static class ChatModule
{
    private static readonly Assembly _module = typeof(ChatModule).Assembly;

    public static void AddChat(this IHostApplicationBuilder builder)
    {
        builder
            .Services
            .AddHttpContextAccessor();

        builder
            .Services
            .AddValidatorsFromAssembly(_module);

        builder
            .Services
            .AddScoped<
                IIntentModelTrainer,
                IntentModelTrainer>();

        // outer modules registration
        builder.Services.AddScoped<
                            IServiceInformationProvider,
                            ServiceInformationProvider>();

        string connectionString =
            builder.Configuration.GetConnectionString("CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "ConnectionStrings:CONNECTION_STRING is not configured.");

        builder.Services.AddDbContext<ChatDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(
                        "__EFMigrationsHistory", Schemas.Chat)));

        // ML training
        builder.Services.AddScoped<
            IIntentModelTrainer,
            IntentModelTrainer>();

        builder.Services.AddHostedService<
            IntentModelInitializer>();

        string modelPath = Path.Combine(
            builder.Environment.ContentRootPath,
            "Models",
            ChatModel.FileName);

        builder.Services
            .AddPredictionEnginePool<ModelInput, ModelOutput>()
            .FromFile(
                modelName: ChatModel.Name,
                filePath: modelPath,
                watchForChanges: true);

        builder.Services.AddSingleton<
            IIntentClassifier,
            MlNetIntentClassifier>();

        builder.Services.AddScoped<ChatDataSeeder>();

        builder.Services.AddScoped<
            IIntentModelTrainer,
            IntentModelTrainer>();

        builder.Services.AddHostedService<
            IntentModelInitializer>();
    }

    public static void MapChatEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints
            .MapGroup("/api/chat")
            .WithTags("Chat");

        group.MapPost("/ask", Ask);
    }

    private static async Task<IResult> Ask(
        ChatRequestDto request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new AskChatCommand(request);

        Result<ChatResponseDto> result = await sender.Send(
            command,
            cancellationToken);
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return result.Error.Type switch
        {
            ErrorType.Validation => Results.BadRequest(result.Errors),
            ErrorType.NullValue => Results.NotFound("Ezt most nem tudom megválaszolni."),
            _ => Results.Problem(result.Errors.FirstOrDefault()?.Description)
        };
    }
}
