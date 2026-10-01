namespace Modules.Service.Messaging;

internal sealed class UpdateOrderStatusCommandHandler(
    ServiceDbContext context)
    : ICommandHandler<UpdateOrderStatusCommand>
{
    public async Task<Result> Handle(
        UpdateOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        Domain.Order? order =
            await context.Orders
                .FirstOrDefaultAsync(
                    x => x.Id == request.OrderId,
                    cancellationToken);

        if (order is null)
        {
            return Result.Failure(
                ErrorType.NotFound);
        }

        order.ChangeStatus(
            request.OrderStatusCode);

        await context.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}
