using Common.Contracts.IntegrationEvents;

namespace Modules.Notification.Infrastructure.Messaging.Consumers;

internal sealed class ContactMessageCreatedIntegrationEventConsumer(INotificationSender notificationSender)
    : IConsumer<ContactMessageCreatedIntegrationEvent>
{
    public async Task Consume(
        ConsumeContext<ContactMessageCreatedIntegrationEvent> context)
    {
        ContactMessageCreatedIntegrationEvent message = context.Message;

        await notificationSender.SendContactMessageAsync(
            message.Name,
            message.Email,
            message.PhoneNumber,
            message.Message,
            message.AttachmentId,
            message.Topic,
            context.CancellationToken);

        await context.Publish(
            new ContactMessageNotificationSentIntegrationEvent(
                message.ContactMessageId),
            context.CancellationToken);
    }
}
