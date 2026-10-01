namespace Modules.WebSetting;

public static class WebSettingModule
{
    private static readonly Assembly _module = typeof(WebSettingModule).Assembly;

    public static void AddWebSetting(this IHostApplicationBuilder builder)
    {
        builder
            .Services
            .AddHttpContextAccessor();

        builder
            .Services
            .AddValidatorsFromAssembly(_module);

        string connectionString =
            builder
                .Configuration
                .GetConnectionString("CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "ConnectionStrings:CONNECTION_STRING is not configured.");

        builder
            .Services
            .AddDbContext<WebSettingDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(
                        "__EFMigrationsHistory",
                        Schemas.WebSetting)));

        builder
            .Services
            .AddScoped<WebSettingDataSeeder>();

        builder
            .Services
            .AddHostedService<WebSettingInitializer>();
    }


    public static void MapWebSettingEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints
            .MapGroup("/api/websetting")
            .WithTags("WebSetting");

        group
            .MapGet("/queue/{key}", Queue);
    }

    private static async Task<IResult> Queue(
        string key,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new IdentifyKeyCommand(key);
        Result<WebSettingValueResponse> result = await sender.Send(
                    command,
                    cancellationToken);
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return result.Error.Type switch
        {
            ErrorType.Validation => Results.BadRequest(result.Errors),
            ErrorType.NullValue => Results.NotFound(result.Errors),
            _ => Results.Problem(result.Errors.FirstOrDefault()?.Description)
        };
    }
}
