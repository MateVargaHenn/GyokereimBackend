

namespace Modules.Contact.Infrastructure;

public sealed class ContactMessageNotificationSentIntegrationEventConsumer(
    ContactDbContext dbContext)
    : IConsumer<ContactMessageNotificationSentIntegrationEvent>
{
    public async Task Consume(
        ConsumeContext<ContactMessageNotificationSentIntegrationEvent> context)
    {
        ContactMessage? contactMessage =
            await dbContext.ContactMessages
                .FirstOrDefaultAsync(
                    x => x.Id == context.Message.ContactMessageId,
                    context.CancellationToken);


        if (contactMessage is null)
        {
            return;
        }


        contactMessage.MarkNotificationAsSent();


        await dbContext.SaveChangesAsync(
            context.CancellationToken);
    }
}
