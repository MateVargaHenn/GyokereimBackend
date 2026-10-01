namespace Modules.Contact.Message;

public sealed class SendContactMessageRequestDto
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;

    public IFormFile? Attachment { get; init; }

    public string CaptchaToken { get; init; } = string.Empty;
}
