namespace Modules.Service.Messaging;

internal sealed class CreateServiceCommandValidator
    : AbstractValidator<CreateServiceCommand>
{
    private readonly ServiceDbContext _context;

    public CreateServiceCommandValidator(
        ServiceDbContext context)
    {
        _context = context;

        RuleFor(x => x.ServiceDto.Name)
            .NotEmpty();

        RuleFor(x => x.ServiceDto.ShortDescription)
            .NotEmpty();

        RuleFor(x => x.ServiceDto.Description)
            .NotEmpty();

        RuleFor(x => x.ServiceDto.Price)
            .GreaterThan(0);

        RuleFor(x => x.ServiceDto.ServiceStatusCode)
            .Must(Enum.IsDefined)
            .WithMessage(
                "Érvénytelen szolgáltatás státusz.");

        RuleFor(x => x.ServiceDto.Slug)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage(
                "A slug csak kisbetűket, számokat és kötőjeleket tartalmazhat.")
            .MustAsync(BeUniqueSlugAsync)
            .WithMessage(
                "Ezzel a sluggal már létezik szolgáltatás.");
    }

    private async Task<bool> BeUniqueSlugAsync(
        string slug,
        CancellationToken cancellationToken) => !await _context.Services
            .AsNoTracking()
            .AnyAsync(
                x => x.Slug == slug,
                cancellationToken);
}
