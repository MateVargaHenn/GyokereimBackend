namespace Modules.Blog.Contract;

public sealed record PostDetailsDto(
    int Id,
    string AuthorId,
    string Title,
    string MetaTitle,
    string Slug,
    string Summary,
    string Content,
    PostStatus PostStatus,
    DateTime? PublishedAt,
    IReadOnlyCollection<PostMetaDto> PostMetas,
    IReadOnlyCollection<PostTagDto> PostTags);
