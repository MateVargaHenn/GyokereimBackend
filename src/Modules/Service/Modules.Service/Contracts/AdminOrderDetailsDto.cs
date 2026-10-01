namespace Modules.Service.Contracts;

public sealed record AdminOrderDetailsDto(
    int Id,
    string CustomerName,
    string Email,
    string PhoneNumber,
    string? Message,
    string ServiceName,
    string TrackingCode,
    OrderStatusCode OrderStatusCode,
    DateTime Created,
    DateTime LastModified);
