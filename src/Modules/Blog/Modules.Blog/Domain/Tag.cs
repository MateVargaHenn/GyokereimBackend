namespace Modules.Blog.Domain;

internal sealed class Tag : Entity
{
    public string Name { get; private set; } = default!;
    public string Slug { get; private set; } = default!;

    private readonly List<PostTag> _postTags = [];

    public IReadOnlyCollection<PostTag> PostTags =>
        _postTags;

    private Tag()
    {
    }

    public static Tag Create(
        string name,
        string slug)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        return new Tag
        {
            Name = name,
            Slug = slug
        };
    }

    public void ChangeName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
    }

    public void ChangeSlug(string slug)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        Slug = slug;
    }
}
