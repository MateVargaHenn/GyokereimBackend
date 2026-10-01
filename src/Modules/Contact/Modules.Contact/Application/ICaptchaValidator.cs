namespace Modules.Contact.Application;

public interface ICaptchaValidator
{
    Task<bool> ValidateAsync(
        string token,
        CancellationToken cancellationToken = default);
}
