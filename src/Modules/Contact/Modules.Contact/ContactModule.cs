using Modules.Contact.Infrastructure;
using Modules.Contact.Infrastructure.Captcha;
using Modules.File.Contract;

namespace Modules.Contact;

public static class ContactModule
{
    private static readonly Assembly _module = 
        typeof(ContactModule).Assembly;

    public static void AddContact(
        this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddHttpContextAccessor();

        builder.Services
            .AddValidatorsFromAssembly(_module);

        string connectionString =
            builder.Configuration
                .GetConnectionString("CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "ConnectionStrings:CONNECTION_STRING is not configured.");

        builder.Services.AddDbContext<ContactDbContext>(
            options => options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                        npgsqlOptions.MigrationsHistoryTable(
                            "__EFMigrationsHistory",
                            Schemas.Contact)));
        builder.Services
    .AddHostedService<ContactInitializer>();
        builder
            .Services
            .AddScoped<
                IIntegrationEventPublisher,
                MassTransitIntegrationEventPublisher>();
        builder.Services.AddHttpClient<
    ICaptchaValidator,
    CloudflareCaptchaValidator>();

        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(_module));
    }

    public static void AddContactConsumers(
        this IBusRegistrationConfigurator bus) => bus.AddConsumers(
    typeof(ContactMessageNotificationSentIntegrationEventConsumer)
        .Assembly);

    public static void MapContactEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints
            .MapGroup("/api/contact")
            .WithTags("Contact");

        group.MapPost("message/send", SendMessage)
            .DisableAntiforgery()
            .RequireRateLimiting("contact");
    }

    private static async Task<IResult> SendMessage(
    [FromForm] SendContactMessageRequestDto request,
    ISender sender,
    CancellationToken cancellationToken)
    {
        FileUpload? attachment = null;
        if (request.Attachment is not null)
        {
            await using Stream stream =
                request.Attachment.OpenReadStream();

            using var memoryStream = new MemoryStream();

            await stream.CopyToAsync(
                memoryStream,
                cancellationToken);

            attachment = new FileUpload(
                request.Attachment.FileName,
                request.Attachment.ContentType,
                memoryStream.ToArray());
        }
        var command = new SendContactMessageCommand(
                            request.Name,
                            request.Email,
                            request.PhoneNumber,
                            request.Message,
                            attachment,
                            request.CaptchaToken,
                            request.Topic);
        
        Result result = await sender.Send(
            command,
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok()
            : Results.BadRequest(result);
    }
}
