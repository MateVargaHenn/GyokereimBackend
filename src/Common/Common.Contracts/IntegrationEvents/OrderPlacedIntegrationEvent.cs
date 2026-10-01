namespace Common.Contracts.IntegrationEvents;

public sealed record OrderPlacedIntegrationEvent(
    int OrderId,
    string CustomerName,
    string Email,
    string PhoneNumber,
    string? Message,
    string ServiceName,
    decimal Price,
    string TrackingLink,
    int ServiceId);
