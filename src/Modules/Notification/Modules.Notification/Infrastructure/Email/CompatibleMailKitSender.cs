using FluentEmail.Core;
using FluentEmail.Core.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Modules.Notification.Infrastructure.Email;

internal sealed class CompatibleMailKitSender(
    string server,
    int port,
    string user,
    string password,
    bool requiresAuthentication)
    : FluentEmail.Core.Interfaces.ISender
{
    public SendResponse Send(
        IFluentEmail email,
        CancellationToken? token = null) => SendAsync(email, token).GetAwaiter().GetResult();

    public async Task<SendResponse> SendAsync(
        IFluentEmail email,
        CancellationToken? token = null)
    {
        CancellationToken cancellationToken = token ?? default;
        cancellationToken.ThrowIfCancellationRequested();

        EmailData data = email.Data;

        using var message = new MimeMessage
        {
            Subject = data.Subject ?? string.Empty
        };

        message.From.Add(new MailboxAddress(
            data.FromAddress.Name,
            data.FromAddress.EmailAddress));

        foreach (Address? address in data.ToAddresses)
            message.To.Add(new MailboxAddress(
                address.Name, address.EmailAddress));

        foreach (Address? address in data.CcAddresses)
            message.Cc.Add(new MailboxAddress(
                address.Name, address.EmailAddress));

        foreach (Address? address in data.BccAddresses)
            message.Bcc.Add(new MailboxAddress(
                address.Name, address.EmailAddress));

        foreach (Address? address in data.ReplyToAddresses)
            message.ReplyTo.Add(new MailboxAddress(
                address.Name, address.EmailAddress));

        var body = new BodyBuilder();

        if (data.IsHtml)
        {
            body.HtmlBody = data.Body;
            body.TextBody = data.PlaintextAlternativeBody;
        }
        else
        {
            body.TextBody = data.Body;
        }

        foreach (Attachment? attachment in data.Attachments)
        {
            MimeEntity part = body.Attachments.Add(
                attachment.Filename,
                attachment.Data,
                ContentType.Parse(attachment.ContentType));

            if (!string.IsNullOrWhiteSpace(attachment.ContentId))
                part.ContentId = attachment.ContentId;
        }

        message.Body = body.ToMessageBody();

        foreach (KeyValuePair<string, string> header in data.Headers)
            message.Headers.Add(header.Key, header.Value);

        using var client = new SmtpClient();

        await client.ConnectAsync(
            server,
            port,
            SecureSocketOptions.SslOnConnect,
            cancellationToken);

        if (requiresAuthentication)
        {
            await client.AuthenticateAsync(
                user,
                password,
                cancellationToken);
        }

        await client.SendAsync(message, cancellationToken);

        // A using lezárja a kapcsolatot.
        // A küldési hibák kivételként továbbmennek a consumerhez.
        return new SendResponse();
    }
}
