using Common.Contracts.IntegrationEvents;

namespace Modules.Notification.Infrastructure.Messaging.Consumers;

internal sealed class OrderPlacedIntegrationEventConsumer(
    INotificationSender notificationSender)
    : IConsumer<OrderPlacedIntegrationEvent>
{
    public async Task Consume(
        ConsumeContext<OrderPlacedIntegrationEvent> context)
  {
        OrderPlacedIntegrationEvent message = context.Message;

        var notification = new OrderNotification(
            message.OrderId,
            message.CustomerName,
            message.Email,
            message.PhoneNumber,
            message.Message,
            message.ServiceName,
            message.Price,
            message.ServiceId,
            message.TrackingLink);

        await notificationSender.SendOrderConfirmationAsync(
            notification,
            context.CancellationToken);

        await notificationSender.SendNewOrderNotificationAsync(
            notification,
            context.CancellationToken);
    }
}
