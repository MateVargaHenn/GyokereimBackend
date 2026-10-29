namespace Modules.Notification.Application.Abstractions;

public interface INotificationSender
{
    Task SendOrderConfirmationAsync(
        OrderNotification notification,
        CancellationToken cancellationToken);

    Task SendNewOrderNotificationAsync(
        OrderNotification notification,
        CancellationToken cancellationToken);

    Task SendContactMessageAsync(
        string name,
        string email,
        string phoneNumber,
        string message,
        int? attachmentId,
        string topic,
        CancellationToken cancellationToken = default);
}
