using Common.Contracts.IntegrationEvents;

namespace Modules.Contact.Domain.DomainEvent;

internal sealed class ContactMessageCreatedDomainEventHandler(
    IIntegrationEventPublisher integrationEventPublisher)
    : INotificationHandler<ContactMessageCreatedDomainEvent>
{
    public async Task Handle(
        ContactMessageCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        ContactMessage contactMessage =
            notification.ContactMessage;

        var integrationEvent =
            new ContactMessageCreatedIntegrationEvent(
                contactMessage.Id,
                contactMessage.Name,
                contactMessage.Email,
                contactMessage.PhoneNumber,
                contactMessage.Message,
                contactMessage.Topic,
                contactMessage.AttachmentId);

        await integrationEventPublisher.PublishAsync(
            integrationEvent,
            cancellationToken);
    }
}
