using Modules.File.Contract;

namespace Modules.Notification.Infrastructure.Email;

internal sealed class EmailNotificationSender(
    IFluentEmailFactory fluentEmailFactory,
    IFileReader fileReader)
    : INotificationSender
{
    public async Task SendOrderConfirmationAsync(
        OrderNotification notification,
        CancellationToken cancellationToken)
    {
        IFluentEmail email = fluentEmailFactory.Create();

        SendResponse response = await email
            .To(notification.Email, notification.CustomerName)
            .Subject("Megrendelés visszaigazolása")
            .UsingTemplateFromFile(
                "Templates/OrderConfirmation.cshtml",
                notification)
            .SendAsync(cancellationToken);

        Console.WriteLine(response);
    }

    public async Task SendNewOrderNotificationAsync(
        OrderNotification notification,
        CancellationToken cancellationToken)
    {
        IFluentEmail email = fluentEmailFactory.Create();

        SendResponse response = await email
            .To("info@gyokereim.hu", "Gyökereim")
            .Subject("Új megrendelés érkezett")
            .UsingTemplateFromFile(
                "Templates/NewOrderNotification.cshtml",
                notification)
            .SendAsync(cancellationToken);

        Console.WriteLine(response);
    }

    public async Task SendContactMessageAsync(
    string name,
    string email,
    string phoneNumber,
    string message,
    int? attachmentId,
    string topic,
    CancellationToken cancellationToken = default)
    {
        var contactMessage = new ContactMessageNotification
        {
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber,
            Message = message
        };
        string subject = topic switch
        {
            "package_selection" =>
                $"Segítség a csomagválasztáshoz - {name}",

            _ => $"Új kapcsolatfelvétel - {name}"
        };

        IFluentEmail fluentEmail = fluentEmailFactory
            .Create()
            .To("info@gyokereim.hu")
            .Subject(subject)
            .UsingTemplateFromFile(
                "Templates/Contact.cshtml",
                contactMessage);

        if (attachmentId.HasValue)
        {
            FileData? attachment = await fileReader.GetFileAsync(
                attachmentId.Value,
                cancellationToken);

            if (attachment is not null)
            {
                fluentEmail.Attach(new Attachment
                {
                    Data = attachment.Content,
                    Filename = attachment.FileName,
                    ContentType = attachment.ContentType
                });
            }
        }

        SendResponse response =
            await fluentEmail.SendAsync(cancellationToken);

        if (!response.Successful)
        {
            throw new InvalidOperationException(
                string.Join(
                    Environment.NewLine,
                    response.ErrorMessages));
        }
    }
}
