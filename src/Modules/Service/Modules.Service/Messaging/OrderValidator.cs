using System.Text.RegularExpressions;

namespace Modules.Service.Messaging;

internal class OrderValidator : AbstractValidator<OrderDto>
{
    public OrderValidator()
    {
        RuleFor(
            x => 
            x.CustomerName)
            .NotEmpty()
            .WithMessage("A 'Név' mező nem lehet üres!");

        RuleFor(
            x =>
            x.Email)
            .NotEmpty()
            .WithMessage("Az 'Email' mező nem lehet üres!")
            .EmailAddress()
            .WithMessage("Érvényes E-mail címet kell megadni!");

        RuleFor(
            x =>
            x.PhoneNumber)
            .NotEmpty()
            .WithMessage("A 'Telefonszám' mező nem lehet üres!")
            .Must(TelephoneNumber)
            .WithMessage("Érvényes telefonszámot kell megadni!");

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
