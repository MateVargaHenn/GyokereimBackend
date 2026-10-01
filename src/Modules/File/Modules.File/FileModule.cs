using Common.SharedKernel.Behavior;
using Common.SharedKernel.Common;
using MassTransit.Middleware;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.File.Infrastructure.Options;
using Modules.File.Messaging;
using Modules.File.Storage;

namespace Modules.File;

public static class FileModule
{
    private static readonly Assembly _module = typeof(FileModule).Assembly;

    public static void AddFile(this IHostApplicationBuilder builder)
    {
        builder
            .Services
            .AddHttpContextAccessor();

        builder
            .Services
            .AddValidatorsFromAssembly(_module);

        string connectionString =
            builder.Configuration.GetConnectionString("CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "ConnectionStrings:CONNECTION_STRING is not configured.");

        builder.Services.AddDbContext<FileDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(
                        "__EFMigrationsHistory", Schemas.File)));

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(_module);

            config.AddOpenBehavior(
                typeof(ValidationBehavior<,>));
        });

        builder.Services
                    .AddHostedService<FileInitializer>();

        builder.Services.AddScoped<IFileModule, UploadFileModule>();
        builder.Services.AddScoped<IFileReader, FileReader>();
        builder.Services.Configure<FileStorageOptions>(
        builder.Configuration.GetSection(FileStorageOptions.SectionName));

        builder.Services.AddSingleton<IFileStorage, LocalFileStorage>();
    }

    public static void MapFileEndpoints(
    this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints
                            .MapGroup("/api/files")
                            .WithTags("File");

        group.MapGet(
                    "admin",
                    GetAdminFiles)
                    .RequireAuthorization(policy =>
                        policy.RequireRole(
                            "Admin",
                            "SuperAdmin"));
        group.MapPost(
                    "admin",
                    UploadFile)
                    .RequireAuthorization(policy =>
                        policy.RequireRole(
                            "Admin",
                            "SuperAdmin"));

        group.MapPut(
                "admin/{id:int}/publish",
                PublishFile)
                .RequireAuthorization(policy =>
                    policy.RequireRole(
                        "Admin",
                        "SuperAdmin"));

        group.MapPut(
            "admin/{id:int}/unpublish",
            UnpublishFile)
            .RequireAuthorization(policy =>
                policy.RequireRole(
                    "Admin",
                    "SuperAdmin"));

        group.MapGet(
                        "{key}",
                        GetPublicFile);
        group.MapGet(
            "admin/{id:int}",
            GetAdminFile)
            .RequireAuthorization(policy =>
                policy.RequireRole(
                    "Admin",
                    "SuperAdmin"));
        group.MapPut(
            "admin/{id:int}",
            UpdateFileMetadata)
            .RequireAuthorization(policy =>
                policy.RequireRole(
                    "Admin",
                    "SuperAdmin"));
    }

    private static async Task<IResult> UpdateFileMetadata(
    int id,
    UpdateFileMetadataDto dto,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result result = await sender.Send(
            new UpdateFileMetadataCommand(
                id,
                dto),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.NotFound(result.Error);
    }

    private static async Task<IResult> GetAdminFile(
    int id,
    IFileReader fileReader,
    CancellationToken cancellationToken)
    {
        FileData? file = await fileReader.GetFileAsync(
            id,
            cancellationToken);

        if (file is null)
        {
            return Results.NotFound();
        }

        return Results.File(
            file.Content,
            file.ContentType,
            enableRangeProcessing: true);
    }

    private static async Task<IResult> UnpublishFile(
    int id,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result result =
            await sender.Send(
                new UnpublishFileCommand(id),
                cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.NotFound(result.Error);
    }

    private static async Task<IResult> PublishFile(
    int id,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result result =
            await sender.Send(
                new PublishFileCommand(id),
                cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.NotFound(result.Error);
    }

    private static async Task<IResult> GetFile(
    string key,
    IFileReader fileReader,
    CancellationToken cancellationToken)
    {
        FileData? file = await fileReader.GetFileAsync(
            key,
            cancellationToken);

        if (file is null)
        {
            return Results.NotFound();
        }

        return Results.File(
            file.Content,
            file.ContentType,
            enableRangeProcessing: true);
    }

    private static async Task<IResult> GetPublicFile(
    string key,
    IFileReader fileReader,
    CancellationToken cancellationToken)
    {
        FileData? file = await fileReader.GetFileAsync(
            key,
            cancellationToken);

        if (file is null)
        {
            return Results.NotFound();
        }

        return Results.File(
            file.Content,
            file.ContentType,
            enableRangeProcessing: true);
    }

    private static async Task<IResult> UploadFile(
    HttpRequest request,
    IFileModule fileModule,
    CancellationToken cancellationToken)
    {
        IFormCollection form =
            await request.ReadFormAsync(cancellationToken);

        IFormFile? file =
            form.Files.FirstOrDefault();

        if (file is null ||
            file.Length == 0)
        {
            return Results.BadRequest(
                "Nincs feltöltendő fájl.");
        }

        await using var stream =
            new MemoryStream();

        await file.CopyToAsync(
            stream,
            cancellationToken);

        var fileUpload =
            new FileUpload(
                file.FileName,
                file.ContentType,
                stream.ToArray());

        int fileId =
            await fileModule.SaveFileAsync(
                fileUpload,
                cancellationToken);

        return Results.Ok(
            new
            {
                Id = fileId
            });
    }

    private static async Task<IResult> GetAdminFiles(
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result<List<AdminFileItemDto>> result =
            await sender.Send(
                new AdminFilesQuery(),
                cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.BadRequest(
            result.Error);
    }
}
