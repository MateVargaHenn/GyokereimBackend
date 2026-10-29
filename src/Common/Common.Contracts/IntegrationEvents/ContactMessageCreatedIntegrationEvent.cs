namespace Common.Contracts.IntegrationEvents;

public sealed record ContactMessageCreatedIntegrationEvent(
    int ContactMessageId,
    string Name,
    string Email,
    string PhoneNumber,
    string Message,
    string Topic,
    int? AttachmentId);
