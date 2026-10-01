namespace Modules.Service.Messaging;

internal sealed class AdminOrderDetailsQueryHandler(
    ServiceDbContext context)
    : IQueryHandler<
        AdminOrderDetailsQuery,
        AdminOrderDetailsDto>
{
    public async Task<Result<AdminOrderDetailsDto>> Handle(
        AdminOrderDetailsQuery request,
        CancellationToken cancellationToken)
    {
        AdminOrderDetailsDto? order =
            await context.Orders
                .AsNoTracking()
                .Where(x => x.Id == request.OrderId)
                .Select(x =>
                    new AdminOrderDetailsDto(
                        x.Id,
                        x.CustomerName,
                        x.Email,
                        x.PhoneNumber,
                        x.Message,
                        x.Service.Name,
                        x.TrackingCode,
                        x.OrderStatusCode,
                        x.Created,
                        x.LastModified))
                .FirstOrDefaultAsync(
                    cancellationToken);

        if (order is null)
        {
            return Result.Failure<AdminOrderDetailsDto>(
                Error.NotFound);
        }

        return Result.Success(order);
    }
}
