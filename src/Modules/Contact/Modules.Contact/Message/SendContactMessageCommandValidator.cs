using System.Text.RegularExpressions;

namespace Modules.Contact.Message;

internal sealed class SendContactMessageCommandValidator
    : AbstractValidator<SendContactMessageCommand>
{
    private static readonly string[] AllowedContentTypes =
    [
        "application/pdf",
        "image/jpeg",
        "image/png",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    ];

    private const int MaxFileSize = 10 * 1024 * 1024;

    public SendContactMessageCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("A 'Név' mező nem lehet üres!")
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Az 'E-mail' mező nem lehet üres!")
            .EmailAddress()
            .WithMessage("Érvényes E-mail címet kell megadni!");

        RuleFor(
            x =>
            x.PhoneNumber)
            .NotEmpty()
            .WithMessage("A 'Telefonszám' mező nem lehet üres!")
            .Must(TelephoneNumber)
            .WithMessage("Érvényes telefonszámot kell megadni!");

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Az 'Üzenet' mező nem lehet üres!");

        When(x => x.Attachment is not null, () =>
        {
            RuleFor(x => x.Attachment!.Content.Length)
                .LessThanOrEqualTo(MaxFileSize)
                .WithMessage("A csatolmány mérete legfeljebb 10 MB lehet.");

            RuleFor(x => x.Attachment!.ContentType)
                .Must(x => AllowedContentTypes.Contains(x))
                .WithMessage("Nem támogatott fájltípus.");
        });
    }

    private bool TelephoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }

        string? normalized = Regex.Replace(phoneNumber, @"[\s\-\(\)]", "");

        return Regex.IsMatch(
            normalized,
            @"^\+[1-9]\d{7,14}$"
        );
    }
}
