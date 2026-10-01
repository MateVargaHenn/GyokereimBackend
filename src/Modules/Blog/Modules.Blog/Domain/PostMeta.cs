namespace Modules.Blog.Domain;

internal sealed class PostMeta : Entity
{
    public int PostId { get; private set; }
    public Post Post { get; private set; } = default!;

    public string Key { get; private set; } = default!;
    public string Value { get; private set; } = default!;

    private PostMeta()
    {
    }

    public static PostMeta Create(
        Post post,
        string key,
        string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        return new PostMeta
        {
            PostId = post.Id,
            Key = key,
            Value = value
        };
    }

    public void ChangeValue(string value) => Value = value;
}
