namespace Modules.Notification.Infrastructure.Email.Models;

public sealed record OrderNotification(
    int OrderId,
    string CustomerName,
    string Email,
    string? PhoneNumber,
    string? Message,
    string ServiceName,
    decimal Price,
    int ServiceId,
    string TrackingLink
);
