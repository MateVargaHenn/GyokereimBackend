using Modules.Service.Application.Abstractions;
using Modules.Service.Contracts;
using Modules.Service.Infrastructure.Catalog;

namespace Modules.Service;

public static class ServiceModule
{
    private static readonly Assembly _module = typeof(ServiceModule).Assembly;

    public static void AddService(this IHostApplicationBuilder builder)
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

        builder.Services.AddDbContext<ServiceDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(
                        "__EFMigrationsHistory", Schemas.Service)));

        builder.Services.AddScoped<ServiceDataSeeder>();
        builder.Services.AddScoped<OrderStatusSeeder>();

        builder
            .Services
            .AddScoped<
                IIntegrationEventPublisher,
                MassTransitIntegrationEventPublisher>();

        builder
            .Services
            .AddScoped<
                IServiceCatalog, 
                ServiceCatalog>();

        builder
            .Services
            .AddHostedService<ServiceInitializer>();
    }

    public static void MapServiceEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints
            .MapGroup("/api/services")
            .WithTags("Service");

        group.MapGet("{key}", QueueWithKey);
        group.MapGet("get", Queue);

        group.MapGet(
    "admin/get",
    GetAdminServices)
    .RequireAuthorization(policy =>
        policy.RequireRole(
            "Admin",
            "SuperAdmin"));

        group.MapGet(
    "admin/{id:int}",
    GetAdminService)
    .RequireAuthorization(policy =>
        policy.RequireRole(
            "Admin",
            "SuperAdmin"));

        group.MapPost(
    "admin",
    CreateService)
    .RequireAuthorization(policy =>
        policy.RequireRole(
            "Admin",
            "SuperAdmin"));

        group.MapPut(
    "admin/{id:int}",
    UpdateService)
    .RequireAuthorization(policy =>
        policy.RequireRole(
            "Admin",
            "SuperAdmin"));

        group.MapPut(
    "admin/{id:int}/deactivate",
    DeactivateService)
    .RequireAuthorization(policy =>
        policy.RequireRole(
            "Admin",
            "SuperAdmin"));

        group.MapPut(
    "admin/{id:int}/activate",
    ActivateService)
    .RequireAuthorization(policy =>
        policy.RequireRole(
            "Admin",
            "SuperAdmin"));

        group.MapPost("order", OrderService);

        group.MapGet("track-order/{trackingNumber}/{email}", TrackOrder);

        group.MapGet(
    "admin/orders",
    GetAdminOrders)
    .RequireAuthorization(policy =>
        policy.RequireRole(
            "Admin",
            "SuperAdmin"));

        group.MapGet(
    "admin/orders/{id:int}",
    GetAdminOrder)
    .RequireAuthorization(policy =>
        policy.RequireRole(
            "Admin",
            "SuperAdmin"));

        group.MapPut(
    "admin/orders/{id:int}/status",
    UpdateOrderStatus)
    .RequireAuthorization(policy =>
        policy.RequireRole(
            "Admin",
            "SuperAdmin"));
    }

    private static async Task<IResult> UpdateOrderStatus(
    int id,
    UpdateOrderStatusDto dto,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result result =
            await sender.Send(
                new UpdateOrderStatusCommand(
                    id,
                    dto.OrderStatusCode),
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
                Results.BadRequest(result.Error),

            _ =>
                Results.BadRequest(result.Error)
        };
    }

    private static async Task<IResult> GetAdminOrder(
    int id,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result<AdminOrderDetailsDto> result =
            await sender.Send(
                new AdminOrderDetailsQuery(id),
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

    private static async Task<IResult> GetAdminOrders(
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result<List<AdminOrderItemDto>> result =
            await sender.Send(
                new AdminOrdersQuery(),
                cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.BadRequest(
            result.Error);
    }

    private static async Task<IResult> ActivateService(
    int id,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result result =
            await sender.Send(
                new ActivateServiceCommand(id),
                cancellationToken);

        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        return result.Error.Type switch
        {
            ErrorType.NotFound =>
                Results.NotFound(result.Error),

            _ =>
                Results.BadRequest(result.Error)
        };
    }

    private static async Task<IResult> DeactivateService(
    int id,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result result =
            await sender.Send(
                new DeactivateServiceCommand(id),
                cancellationToken);

        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        return result.Error.Type switch
        {
            ErrorType.NotFound =>
                Results.NotFound(result.Error),

            _ =>
                Results.BadRequest(result.Error)
        };
    }

    private static async Task<IResult> UpdateService(
    int id,
    CreateServiceDto serviceDto,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result result =
            await sender.Send(
                new UpdateServiceCommand(
                    id,
                    serviceDto),
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
                Results.BadRequest(result.Error),

            _ =>
                Results.BadRequest(result.Error)
        };
    }

    private static async Task<IResult> CreateService(
    CreateServiceDto serviceDto,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result result = await sender.Send(
            new CreateServiceCommand(serviceDto),
            cancellationToken);

        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        return result.Error.Type switch
        {
            ErrorType.Validation =>
                Results.BadRequest(result.Error),

            _ =>
                Results.BadRequest(result.Error)
        };
    }

    private static async Task<IResult> GetAdminService(
    int id,
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result<AdminServiceDto> result =
            await sender.Send(
                new AdminServiceQuery(id),
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

    private static async Task<IResult> GetAdminServices(
    ISender sender,
    CancellationToken cancellationToken)
    {
        Result<List<AdminServiceDto>> result =
            await sender.Send(
                new AdminServicesQuery(),
                cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.BadRequest(result.Error);
    }

    private static async Task<IResult> TrackOrder([AsParameters] TrackOrderRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new TrackOrderQuery(
                            request.TrackingNumber, 
                            request.Email);

        Result<OrderTrackingDto> result = await sender
                                                    .Send(
                                                    query, 
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

    private static async Task<IResult> OrderService([FromBody] OrderDto orderDto,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new OrderCommand(orderDto);

        Result result = await sender
            .Send(command,
                  cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok();
        }

        return result.Error.Type switch
        {
            ErrorType.Validation => Results.BadRequest(result.Error),
            ErrorType.NullValue => Results.NotFound(result.Error),
            _ => Results.Problem(result.Error.Description)
        };
    }

    private static async Task<IResult> Queue(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new ServicesQuery();

        Result<List<ServiceDto>> result = await sender
            .Send(query,
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

    private static async Task<IResult> QueueWithKey(
        string key,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new ServiceQuery(key);

        Result<ServiceDto> result = await sender.Send(
                    query,
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
