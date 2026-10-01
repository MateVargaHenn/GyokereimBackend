namespace Modules.Notification;

public static class NotificationModule
{
    private static readonly Assembly _module = typeof(NotificationModule).Assembly;

    public static void AddNotification(this IHostApplicationBuilder builder)
    {

        builder
            .Services
            .AddHttpContextAccessor();

        builder
            .Services
            .AddValidatorsFromAssembly(_module);
        builder.Services.AddScoped<
    INotificationSender,
    EmailNotificationSender>();

        SmtpSettings smtp = builder.Configuration
    .GetSection("Smtp")
    .Get<SmtpSettings>()
    ?? throw new InvalidOperationException(
        "Az SMTP konfiguráció hiányzik.");

        builder.Services
            .AddFluentEmail(
                smtp.FromEmail,
                smtp.FromName)
            .AddRazorRenderer()
            .AddMailKitSender(
                new SmtpClientOptions
                {
                    Server = smtp.Server,
                    Port = smtp.Port,
                    User = smtp.User,
                    Password = smtp.Password,
                    RequiresAuthentication =
                        smtp.RequiresAuthentication,
                    SocketOptions = MailKit.Security.SecureSocketOptions.SslOnConnect
                });

        builder.Services.AddScoped<
            INotificationSender,
            EmailNotificationSender>();
    }
    public static void AddNotificationConsumers(
        this IBusRegistrationConfigurator bus)
    {
        bus.AddConsumer<OrderPlacedIntegrationEventConsumer>();
        bus.AddConsumer<ContactMessageCreatedIntegrationEventConsumer>();
    }

}
