namespace Modules.Notification.Configurations;

public sealed class SmtpSettings
{
    public string Server { get; set; } = string.Empty;
    public int Port { get; set; }
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;

    public bool RequiresAuthentication { get; set; }
    public bool UseSsl { get; set; }
}
