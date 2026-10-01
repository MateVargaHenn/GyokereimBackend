namespace Modules.Blog.Domain;

internal sealed class PostTag : Entity
{
    public int PostId { get; private set; }
    public Post Post { get; private set; } = default!;

    public int TagId { get; private set; }
    public Tag Tag { get; private set; } = default!;

    public static PostTag Create(
        Post post,
        Tag tag)
        => new()
        {
            Post = post,
            PostId = post.Id,
            Tag = tag,
            TagId = tag.Id
        };
}
