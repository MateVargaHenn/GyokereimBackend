namespace Modules.Service.Messaging;

internal sealed class TrackOrderQueryHandler(ServiceDbContext context) : IQueryHandler<TrackOrderQuery, OrderTrackingDto>
{
    public async Task<Result<OrderTrackingDto>> Handle(
        TrackOrderQuery request, 
        CancellationToken cancellationToken)
    {
        OrderTrackingDto? order = await context.Orders
                            .AsNoTracking()
                            .Where(x =>
                                x.TrackingCode == request.TrackingCode &&
                                x.Email == request.Email)
                            .Select(x => new OrderTrackingDto(
                            
                                x.TrackingCode,
                                x.Service.Name,
                                x.Email,
                                x.OrderStatus.Name,
                                x.Created,
                                x.LastModified
                            ))
                            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return Result.Failure<OrderTrackingDto>(
                Error.NotFound);

        }

        return Result.Success<OrderTrackingDto>(order);
    }
}
