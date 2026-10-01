

namespace Modules.Contact.Infrastructure.Captcha;

internal sealed class CloudflareCaptchaValidator(
    HttpClient httpClient,
    IConfiguration configuration)
    : ICaptchaValidator
{
    public async Task<bool> ValidateAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        string secret =
            configuration["TURNSTILE_SECRET"]
            ?? throw new InvalidOperationException(
                "TURNSTILE_SECRET nincs beállítva.");

        var request = new
        {
            secret,
            response = token
        };

        HttpResponseMessage response =
            await httpClient.PostAsJsonAsync(
                "https://challenges.cloudflare.com/turnstile/v0/siteverify",
                request,
                cancellationToken);

        response.EnsureSuccessStatusCode();

        TurnstileResponse? result =
            await response.Content.ReadFromJsonAsync<TurnstileResponse>(
                cancellationToken: cancellationToken);

        return result?.Success == true;
    }

    private sealed class TurnstileResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; init; }

        [JsonPropertyName("error-codes")]
        public string[] ErrorCodes { get; init; } = [];
    }
}
