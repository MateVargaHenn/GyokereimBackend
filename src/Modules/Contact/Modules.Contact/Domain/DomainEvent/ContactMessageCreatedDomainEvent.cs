namespace Modules.Contact.Domain.DomainEvent;

public sealed record ContactMessageCreatedDomainEvent(
    ContactMessage ContactMessage)
    : IDomainEvent;
