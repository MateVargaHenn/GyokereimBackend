using MassTransit;

namespace Modules.Blog.Message;

internal sealed class CreatePostCommandValidator
    : AbstractValidator<CreatePostCommand>
{
    private readonly BlogDbContext _dbContext;

    public CreatePostCommandValidator(
        BlogDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.AuthorId)
            .NotEmpty();

        RuleFor(x => x.CreatePostDetailsDto.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.CreatePostDetailsDto.MetaTitle)
            .NotEmpty();

        RuleFor(x => x.CreatePostDetailsDto.Slug)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage(
                "A slug csak kisbetűket, számokat és kötőjeleket tartalmazhat.")
            .MustAsync(BeUniqueSlugAsync)
            .WithMessage(
                "Ezzel a sluggal már létezik bejegyzés.");

        RuleFor(x => x.CreatePostDetailsDto.Summary)
            .NotEmpty();

        RuleFor(x => x.CreatePostDetailsDto.Content)
            .NotEmpty();

        RuleFor(x => x.CreatePostDetailsDto.PostMetas)
            .NotNull();

        RuleFor(x => x.CreatePostDetailsDto.PostTags)
            .NotNull();
    }

    private async Task<bool> BeUniqueSlugAsync(
    string slug,
    CancellationToken cancellationToken) => !await _dbContext.Posts
            .AsNoTracking()
            .AnyAsync(
                x => x.Slug == slug,
                cancellationToken);
}
