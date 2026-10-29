using Modules.Contact.Domain.DomainEvent;

namespace Modules.Contact.Domain;

public sealed class ContactMessage : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;

    public int? AttachmentId { get; private set; }

    public bool NotificationSent { get; private set; }

    public int? ReplyToId { get; private set; }
    public string? Topic { get; set; }
    public ContactMessage? Parent { get; private set; }

    private ContactMessage()
    {
    }

    public static ContactMessage Create(
        string name,
        string email,
        string phoneNumber,
        string message,
        string? topic,
        int? attachmentId)
    {
        var contactMessage = new ContactMessage
        {
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber,
            Message = message,
            Topic = topic,
            AttachmentId = attachmentId,
            NotificationSent = false
        };

        contactMessage.RaiseDomainEvent(new ContactMessageCreatedDomainEvent(contactMessage));
        return contactMessage;
    }

    public void MarkNotificationAsSent() 
        => NotificationSent = true;

    public void SetReplyTo(int replyToId) 
        => ReplyToId = replyToId;
}
