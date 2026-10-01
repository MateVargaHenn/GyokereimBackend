namespace Modules.Blog.Domain;

internal class Post : Entity
{
    public string AuthorId { get; private set; } = default!;
    public int? ParentId { get; private set; } = default!;
    public Post? Parent { get; private set; } = default!;

    public string Title { get; private set; } = default!;
    public string MetaTitle { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public string Summary { get; private set; } = default!;
    public PostStatus PostStatus { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? PublishedAt { get; private set; }

    public string Content { get; private set; } = string.Empty;
    private readonly List<PostMeta> _meta = [];
    public IReadOnlyCollection<PostMeta> Meta => _meta;
    private readonly List<PostTag> _postTags = [];
    public IReadOnlyCollection<PostTag> PostTags => _postTags;

    public static Post Create(
        string authorId,
        string title,
        string metaTitle,
        string slug,
        string summary,
        string content)
    {
        if (string.IsNullOrWhiteSpace(authorId))
        {
            throw new InvalidAuthorIdException();
        }
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new PostTitleRequiredException();
        }
        if (title.Length > 200)
        {
            throw new PostTitleTooLongException();
        }
        return new Post
        {
            AuthorId = authorId,
            Title = title,
            MetaTitle = metaTitle,
            Slug = slug,
            Summary = summary,
            Content = content,
            PostStatus = PostStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
    public void ChangeAuthor(string authorId)
    {
        if (string.IsNullOrWhiteSpace(authorId))
        {
            throw new InvalidAuthorIdException();
        }

        AuthorId = authorId;
    }

    public void ChangeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new PostTitleRequiredException();
        }

        if (title.Length > 200)
        {
            throw new PostTitleTooLongException();
        }

        Title = title;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddTag(Tag tag)
    {
        if (_postTags.Any(x => x.TagId == tag.Id))
        {
            return;
        }

        _postTags.Add(
            PostTag.Create(this, tag));
    }

    public void RemoveTag(Tag tag)
    {
        PostTag? postTag =
            _postTags.FirstOrDefault(x => x.TagId == tag.Id);

        if (postTag is not null)
        {
            _postTags.Remove(postTag);
        }
    }

    public void AddMeta(string key, string value)
    {
        PostMeta? existing =
            _meta.FirstOrDefault(x => x.Key == key);

        if (existing is not null)
        {
            existing.ChangeValue(value);
            return;
        }

        _meta.Add(PostMeta.Create(this, key, value));
    }

    public void RemoveMeta(string key)
    {
        PostMeta? meta =
            _meta.FirstOrDefault(x => x.Key == key);

        if (meta is not null)
        {
            _meta.Remove(meta);
        }
    }

    public void ChangeMetaTitle(string metaTitle)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(metaTitle);

        MetaTitle = metaTitle;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeSlug(string slug)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        Slug = slug;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeSummary(string summary)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(summary);

        Summary = summary;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeContent(string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        Content = content;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Publish()
    {
        if (PostStatus == PostStatus.Published)
        {
            return;
        }

        PostStatus = PostStatus.Published;
        PublishedAt ??= DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MoveToDraft()
    {
        if (PostStatus == PostStatus.Draft)
        {
            return;
        }

        PostStatus = PostStatus.Draft;
        UpdatedAt = DateTime.UtcNow;
    }
}
