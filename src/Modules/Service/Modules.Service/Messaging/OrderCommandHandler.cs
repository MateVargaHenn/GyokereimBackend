namespace Modules.Service.Messaging;

internal sealed class OrderCommandHandler(ServiceDbContext context) : ICommandHandler<OrderCommand>
{
    private readonly ServiceDbContext _context = context;
    public async Task<Result> Handle(
    OrderCommand request,
    CancellationToken cancellationToken)
    {
        Domain.Service? service = await _context.Services
            .FirstOrDefaultAsync(
                x => x.Slug == request.OrderDto.ServiceKey,
                cancellationToken);

        if (service is null)
        {
            return Result.Failure(ErrorType.NotFound);
        }

        var order = Domain.Order.Create(
            request.OrderDto.CustomerName,
            request.OrderDto.Email,
            request.OrderDto.PhoneNumber,
            request.OrderDto.Message,
            service.Id);

        await _context.Orders.AddAsync(order, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
