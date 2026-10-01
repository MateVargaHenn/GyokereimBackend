namespace Modules.Service.Messaging;

public sealed record TrackOrderQuery(
    string TrackingCode,
    string Email) : IQuery<OrderTrackingDto>;
