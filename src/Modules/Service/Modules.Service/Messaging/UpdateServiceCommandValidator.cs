namespace Modules.Service.Messaging;

internal sealed class UpdateServiceCommandValidator
    : AbstractValidator<UpdateServiceCommand>
{
    private readonly ServiceDbContext _context;

    public UpdateServiceCommandValidator(
        ServiceDbContext context)
    {
        _context = context;

        RuleFor(x => x.ServiceId)
            .GreaterThan(0);

        RuleFor(x => x.ServiceDto.Name)
            .NotEmpty();

        RuleFor(x => x.ServiceDto.ShortDescription)
            .NotEmpty();

        RuleFor(x => x.ServiceDto.Description)
            .NotEmpty();

        RuleFor(x => x.ServiceDto.Price)
            .GreaterThan(0);

        RuleFor(x => x.ServiceDto.OrderNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.ServiceDto.ServiceStatusCode)
            .Must(status =>
                Enum.IsDefined(status))
            .WithMessage(
                "Érvénytelen szolgáltatás státusz.");

        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .Must(x =>
                !string.IsNullOrWhiteSpace(
                    x.ServiceDto.Slug))
            .WithMessage(
                "A slug megadása kötelező.")
            .Must(x =>
                System.Text.RegularExpressions.Regex
                    .IsMatch(
                        x.ServiceDto.Slug,
                        "^[a-z0-9]+(?:-[a-z0-9]+)*$"))
            .WithMessage(
                "A slug csak kisbetűket, számokat és kötőjeleket tartalmazhat.")
            .MustAsync(BeUniqueSlugAsync)
            .WithMessage(
                "Ezzel a sluggal már létezik másik szolgáltatás.");
    }

    private async Task<bool> BeUniqueSlugAsync(
        UpdateServiceCommand command,
        CancellationToken cancellationToken) => !await _context.Services
            .AsNoTracking()
            .AnyAsync(
                x =>
                    x.Slug == command.ServiceDto.Slug &&
                    x.Id != command.ServiceId,
                cancellationToken);
}
