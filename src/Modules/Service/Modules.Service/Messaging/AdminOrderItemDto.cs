namespace Modules.Service.Messaging;

public sealed record AdminOrderItemDto(
    int Id,
    string CustomerName,
    string Email,
    string PhoneNumber,
    string ServiceName,
    string TrackingCode,
    OrderStatusCode OrderStatusCode,
    DateTime Created);
