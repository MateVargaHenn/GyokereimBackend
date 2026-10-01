namespace Modules.Blog.Message;
internal sealed class UpdatePostCommandValidator
    : AbstractValidator<UpdatePostCommand>
{
    private readonly BlogDbContext _dbContext;

    public UpdatePostCommandValidator(
        BlogDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.PostId)
            .GreaterThan(0);

        RuleFor(x => x.PostDetailsDto.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.PostDetailsDto.MetaTitle)
            .NotEmpty();

        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .Must(x =>
                !string.IsNullOrWhiteSpace(
                    x.PostDetailsDto.Slug))
            .WithMessage(
                "A slug megadása kötelező.")
            .Must(x =>
                System.Text.RegularExpressions.Regex.IsMatch(
                    x.PostDetailsDto.Slug,
                    "^[a-z0-9]+(?:-[a-z0-9]+)*$"))
            .WithMessage(
                "A slug csak kisbetűket, számokat és kötőjeleket tartalmazhat.")
            .MustAsync(BeUniqueSlugAsync)
            .WithMessage(
                "Ezzel a sluggal már létezik másik bejegyzés.");

        RuleFor(x => x.PostDetailsDto.Summary)
            .NotEmpty();

        RuleFor(x => x.PostDetailsDto.Content)
            .NotEmpty();

        RuleFor(x => x.PostDetailsDto.PostMetas)
            .NotNull();

        RuleFor(x => x.PostDetailsDto.PostTags)
            .NotNull();
    }

    private async Task<bool> BeUniqueSlugAsync(
        UpdatePostCommand command,
        CancellationToken cancellationToken) => !await _dbContext.Posts
            .AsNoTracking()
            .AnyAsync(
                x =>
                    x.Slug ==
                        command.PostDetailsDto.Slug &&
                    x.Id != command.PostId,
                cancellationToken);
}
