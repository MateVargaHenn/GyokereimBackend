namespace Modules.Service.Application.DomainEventHandlers;

internal sealed class OrderPlacedDomainEventHandler
    : INotificationHandler<OrderPlacedDomainEvent>
{
    private readonly IIntegrationEventPublisher _publisher;
    private readonly IConfiguration _configuration;

    public OrderPlacedDomainEventHandler(
        IIntegrationEventPublisher publisher, IConfiguration configuration)
    {
        _publisher = publisher;
        _configuration = configuration;
    }

    public async Task Handle(
        OrderPlacedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        Domain.Order order = notification.Order;
        string frontendUrl = _configuration.GetSection("FRONTEND_URL").Value;
        var integrationEvent = new OrderPlacedIntegrationEvent(
            order.Id,
            order.CustomerName,
            order.Email,
            order.PhoneNumber,
            order.Message,
            order.Service.Name,
            order.Service.Price,
            string.Format($"{frontendUrl}/nyomkovetes/{order.TrackingCode}/{order.Email}"),
            order.ServiceId);

        await _publisher.PublishAsync(
            integrationEvent,
            cancellationToken);
    }
}


