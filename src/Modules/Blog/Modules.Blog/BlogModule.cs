using System.Security.Claims;
using Common.Application.Pagination;
using Common.SharedKernel.Behavior;

namespace Modules.Blog;

public static class BlogModule
{
    private static readonly Assembly _module = typeof(BlogModule).Assembly;
    public static void AddBlog(this IHostApplicationBuilder builder)
    {
        builder
            .Services
            .AddHttpContextAccessor();

        builder
            .Services
            .AddValidatorsFromAssembly(
                _module,
                includeInternalTypes: true);

        builder.Services.AddAuthorization(options => options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin", "SuperAdmin")));

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(_module);

            config.AddOpenBehavior(
                typeof(ValidationBehavior<,>));
        });

        string connectionString =
            builder.Configuration.GetConnectionString("CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "ConnectionStrings:CONNECTION_STRING is not configured.");

        builder.Services.AddDbContext<BlogDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(
                        "__EFMigrationsHistory", Schemas.Blog)));

        builder.Services.AddHostedService<BlogInitializer>();

    }
    public static void MapBlogEndpoints(
    this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints
            .MapGroup("/api/blog")
            .WithTags("Blog");

        group.MapGet("/posts-for-home", GetPostsForHome);
        group.MapGet("/posts/{slug}", GetPostBySlug);

        group.MapGet("/posts/{page:int}", GetPublishedPostsForBlog);

        group.MapPost("/admin/post", CreatePost)
            .RequireAuthorization(policy =>
        policy.RequireRole("Admin", "SuperAdmin"));


        group.MapGet(
        "/admin/posts/{page:int}",
        GetPostsForAdmin)
        .RequireAuthorization(policy =>
            policy.RequireRole(
                "Admin",
                "SuperAdmin"));

        group.MapGet(
        "/admin/post/{id:int}",
        GetPostById)
        .RequireAuthorization(policy =>
            policy.RequireRole(
                "Admin",
                "SuperAdmin"));

        group.MapPut(
    "/admin/post/{id:int}",
    UpdatePost)
    .RequireAuthorization(policy =>
        policy.RequireRole(
            "Admin",
            "SuperAdmin"));

        group.MapPut(
    "/admin/post/{id:int}/publish",
    PublishPost)
    .RequireAuthorization(policy =>
        policy.RequireRole("Admin", "SuperAdmin"));

        group.MapPut(
            "/admin/post/{id:int}/draft",
            MovePostToDraft)
            .RequireAuthorization(policy =>
                policy.RequireRole("Admin", "SuperAdmin"));

        group.MapDelete(
            "/admin/post/{id:int}",
            DeletePost)
            .RequireAuthorization(policy =>
                policy.RequireRole("Admin", "SuperAdmin"));
    }

    private static async Task<IResult> UpdatePost(
    int id,
    CreatePostDetailsDto postDetailsDto,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result result = await sender.Send(
            new UpdatePostCommand(
                id,
                postDetailsDto),
            cancellationToken);

        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        return result.Error.Type switch
        {
            ErrorType.NotFound =>
                Results.NotFound(result.Error),

            ErrorType.Validation =>
                Results.BadRequest(result.ValidationError),

            _ =>
                Results.BadRequest(result.Error)
        };
    }

    private static async Task<IResult> DeletePost(
    int id,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result result = await sender.Send(
            new DeletePostCommand(id),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.Error.Type switch
            {
                ErrorType.NotFound =>
                    Results.NotFound(result.Error),

                _ =>
                    Results.BadRequest(result.Error)
            };
    }

    private static async Task<IResult> GetPostsForAdmin(
    int page,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result<PagedResult<AdminPostItemDto>> result =
            await sender.Send(
                new GetPostsForAdminQuery(page),
                cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> MovePostToDraft(
    int id,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result result = await sender.Send(
            new MovePostToDraftCommand(id),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.Error.Type switch
            {
                ErrorType.NotFound =>
                    Results.NotFound(result.Error),

                _ =>
                    Results.BadRequest(result.Error)
            };
    }

    private static async Task<IResult> PublishPost(
    int id,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result result = await sender.Send(
            new PublishPostCommand(id),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.Error.Type switch
            {
                ErrorType.NotFound =>
                    Results.NotFound(result.Error),

                _ =>
                    Results.BadRequest(result.Error)
            };
    }

    private static async Task<IResult> GetPostById(
    int id,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result<PostDetailsDto> result =
            await sender.Send(
                new GetPostByIdQuery(id),
                cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return result.Error.Type switch
        {
            ErrorType.NotFound =>
                Results.NotFound(result.Error),

            _ =>
                Results.BadRequest(result.Error)
        };
    }

    private static async Task<IResult> GetPostsForHome(
        ISender sender,
        CancellationToken cancellationToken)
    {
        Result<IReadOnlyCollection<PostItemDto>> result =
            await sender.Send(
                new GetPublishedPostsForHomeQuery(),
                cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result);
    }


    private static async Task<IResult> GetPublishedPostsForBlog(
        ISender sender,
        int Page,
        CancellationToken cancellationToken)
    {
        Result<PagedResult<PostItemDto>> result = 
            await sender.Send(
                new GetPublishedPostsQuery(Page), 
                cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result);
    }
    private static async Task<IResult> GetPostBySlug(
        string slug, 
        ISender sender, 
        CancellationToken cancellationToken)
    {

        Result<PostDetailsDto> result =
            await sender.Send(
                new GetPostBySlugQuery(slug),
                cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return result.Error.Type switch
        {
            ErrorType.NotFound => Results.NotFound(result.Errors),
            _ => Results.BadRequest(result.Errors)
        };

    }

    private static async Task<IResult> CreatePost(
        CreatePostDetailsDto createPostDetailsDto,
        HttpContext httpContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        string? authorId =
            httpContext.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(authorId))
        {
            return Results.Unauthorized();
        }

        var command = new CreatePostCommand(
            authorId,
            createPostDetailsDto);

        Result result =
            await sender.Send(
                command,
                cancellationToken);

        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        return result.Error.Type switch
        {
            ErrorType.Validation =>
                Results.BadRequest(result.ValidationError),

            _ =>
                Results.BadRequest(result.Error)
        };
    }

}
