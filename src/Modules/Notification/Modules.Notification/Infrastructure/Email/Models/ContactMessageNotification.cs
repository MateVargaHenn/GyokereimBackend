namespace Modules.Notification.Infrastructure.Email.Models;

public class ContactMessageNotification
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Message { get; set; }
}
