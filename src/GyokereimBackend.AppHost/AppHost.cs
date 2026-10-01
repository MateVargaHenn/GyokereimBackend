IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);
IResourceBuilder<ParameterResource> superAdminUser =
    builder.AddParameter("superadmin-user");

IResourceBuilder<ParameterResource> superAdminPassword =
    builder.AddParameter("superadmin-password", secret: true);

IResourceBuilder<ParameterResource> superAdminEmail =
    builder.AddParameter("superadmin-email");

IResourceBuilder<ParameterResource> adminUser =
    builder.AddParameter("admin-user");

IResourceBuilder<ParameterResource> adminPassword =
    builder.AddParameter("admin-password", secret: true);

IResourceBuilder<ParameterResource> adminEmail =
    builder.AddParameter("admin-email");
IResourceBuilder<ParameterResource> postgresUser = builder.AddParameter("postgres-user");
IResourceBuilder<ParameterResource> postgresPassword = builder.AddParameter(
    "postgres-password",
    secret: true);


IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("postgres",
        userName: postgresUser,
        password: postgresPassword)
    .WithHostPort(5432)
    .WithImageTag("18")
    .WithDataVolume()
    .WithPgWeb();

IResourceBuilder<PostgresDatabaseResource> database = postgres.AddDatabase(
    "gyokereim",
    databaseName: "gyokereim");

IResourceBuilder<RabbitMQServerResource> rabbitMq = builder
    .AddRabbitMQ("RABBITMQ")
    .WithManagementPlugin()
    .WithDataVolume();

IResourceBuilder<SeqResource> seq = builder
    .AddSeq("seq")
    .WithDataVolume()
    .ExcludeFromManifest()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEnvironment("ACCEPT_EULA", "Y");

IResourceBuilder<ParameterResource> smtpPassword = builder.AddParameter(
    "smtp-password",
    secret: true);

builder
    .AddProject<Projects.WebApi>("webapi")
    .WithHttpEndpoint(port: 3000)
    .WithReference(database, "CONNECTION_STRING")

    .WithReference(rabbitMq)
    .WithReference(seq)
    //smtp:
    .WithEnvironment("Smtp__Server", "smtp.forpsi.com")
    .WithEnvironment("Smtp__Port", "465")
    .WithEnvironment("Smtp__User", "nevalaszolj@gyokereim.hu")
    .WithEnvironment("Smtp__Password", smtpPassword)
    .WithEnvironment("Smtp__FromEmail", "nevalaszolj@gyokereim.hu")
    .WithEnvironment("Smtp__FromName", "Gyökereim")
    .WithEnvironment("Smtp__RequiresAuthentication", "true")
    .WithEnvironment("Smtp__UseSsl", "true")

    .WithEnvironment("FRONTEND_URL", "http://localhost:4200")
    .WithEnvironment("ADMIN_FRONTEND_URL", "http://localhost:4201")

    .WithEnvironment(
        "APP__SUPERADMIN__USER",
        superAdminUser)
    .WithEnvironment(
        "APP__SUPERADMIN__PASSWORD",
        superAdminPassword)
    .WithEnvironment(
        "APP__SUPERADMIN__EMAIL",
        superAdminEmail)
    .WithEnvironment(
        "APP__ADMIN__USER",
        adminUser)
    .WithEnvironment(
        "APP__ADMIN__PASSWORD",
        adminPassword)
    .WithEnvironment(
        "APP__ADMIN__EMAIL",
        adminEmail)
    .WaitFor(database)
    .WaitFor(rabbitMq)
    .WaitFor(seq);


await builder.Build().RunAsync();
