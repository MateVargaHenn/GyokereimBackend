using System.Threading.RateLimiting;
using DocumentFormat.OpenXml.InkML;
using Modules.Authentication;
using Modules.Blog;
using Modules.File;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    builder.AddAuthentication();
    builder.AddWebSetting();
    builder.AddService();
    builder.AddFile();
    builder.AddContact();
    builder.AddBlog();
    builder.AddChat();
    builder.AddNotification();

    // Host
    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        string seqServerUrl =
        context.Configuration["SEQ_URI"] ?? context.Configuration["Seq:ServerUrl"]
            ?? throw new InvalidOperationException(
                "A Seq:ServerUrl környezeti változó nincs beállítva.");

        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq(seqServerUrl);
    });

    string frontendUrl =
    builder.Configuration["FRONTEND_URL"]
    ?? throw new Exception("A FRONTEND_URL környezeti változó nincs beállítva.");

    string adminFrontendUrl =
    builder.Configuration["ADMIN_FRONTEND_URL"]
    ?? throw new InvalidOperationException(
        "Az ADMIN_FRONTEND_URL környezeti változó nincs beállítva.");

    builder.Services.AddCors(options => options.AddPolicy("Frontend", policy => policy
                .WithOrigins(frontendUrl, adminFrontendUrl)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));
    // Add services to the container.
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });
    builder.Services.AddRateLimiter(options => options.AddPolicy("contact", httpContext =>
        {
            string ipAddress =
                httpContext.Connection.RemoteIpAddress?.ToString()
                ?? "unknown";

            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: ipAddress,
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0
                });
        }));
    builder.Services.AddAntiforgery();
    builder.Services.AddScoped<
    IDomainEventsDispatcher,
    DomainEventsDispatcher>();
    builder.Services.AddMassTransit(x =>
    {
        x.AddNotificationConsumers();
        x.AddContactConsumers();

        x.UsingRabbitMq((context, cfg) =>
        {
            string? connectionString =
                builder.Configuration.GetConnectionString("RABBITMQ");

            cfg.Host(new Uri(connectionString!));

            cfg.ConfigureEndpoints(context);
        });
    });

    string? licenseKey = Environment.GetEnvironmentVariable("MEDIATR_LICENSE_KEY");
    builder.Services.AddMediatR(configuration =>
    {
        configuration.RegisterServicesFromAssemblies(
            typeof(Modules.Chat.AssemblyReference).Assembly,
            typeof(Modules.WebSetting.AssemblyReference).Assembly,
            typeof(Modules.Service.AssemblyReference).Assembly
            );
        configuration.LicenseKey = licenseKey;
    });
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
}
WebApplication app = builder.Build();
{
    app.Lifetime.ApplicationStarted.Register(() =>
        app.Logger.LogInformation("Az alkalmazás elindult."));


    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    app.UseCors("Frontend");


    app.UseRouting();

    app.UseSerilogRequestLogging();
    app.UseAntiforgery();
    app.UseRateLimiter();
    app.UseExceptionHandler();
    app.MapAuthenticationEndpoints();
    app.MapWebSettingEndpoints();
    app.MapChatEndpoints();
    app.MapServiceEndpoints();
    app.MapContactEndpoints();
    app.MapBlogEndpoints();
    app.MapFileEndpoints();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    await app.RunAsync();
}
