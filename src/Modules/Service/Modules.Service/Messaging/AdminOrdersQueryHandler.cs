namespace Modules.Service.Messaging;

internal sealed class AdminOrdersQueryHandler(
    ServiceDbContext context)
    : IQueryHandler<
        AdminOrdersQuery,
        List<AdminOrderItemDto>>
{
    public async Task<Result<List<AdminOrderItemDto>>> Handle(
        AdminOrdersQuery request,
        CancellationToken cancellationToken)
    {
        List<AdminOrderItemDto> orders =
            await context.Orders
                .AsNoTracking()
                .OrderByDescending(x => x.Created)
                .Select(x =>
                    new AdminOrderItemDto(
                        x.Id,
                        x.CustomerName,
                        x.Email,
                        x.PhoneNumber,
                        x.Service.Name,
                        x.TrackingCode,
                        x.OrderStatusCode,
                        x.Created))
                .ToListAsync(cancellationToken);

        return Result
            .Success<List<AdminOrderItemDto>>(
                orders);
    }
}
