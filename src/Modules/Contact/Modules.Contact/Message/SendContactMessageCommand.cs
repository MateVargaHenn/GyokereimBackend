using Modules.File.Contract;

namespace Modules.Contact.Message;

internal sealed record SendContactMessageCommand(string Name,
    string Email,
    string PhoneNumber,
    string Message,
    FileUpload? Attachment,
    string? CaptchaToken) : ICommand;
