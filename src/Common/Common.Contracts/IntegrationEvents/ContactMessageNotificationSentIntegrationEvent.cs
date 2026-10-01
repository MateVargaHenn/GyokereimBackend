namespace Common.Contracts.IntegrationEvents;

public sealed record ContactMessageNotificationSentIntegrationEvent(
    int ContactMessageId);
