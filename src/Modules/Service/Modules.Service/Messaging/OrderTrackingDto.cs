namespace Modules.Service.Messaging;

public record OrderTrackingDto(
    string TrackingNumber,
    string ServiceName,
    string Email,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt);
