namespace Modules.Blog.Contract;

public sealed record CreatePostDetailsDto(
    string Title,
    string MetaTitle,
    string Slug,
    string Summary,
    string Content,
    IReadOnlyCollection<PostMetaDto> PostMetas,
    IReadOnlyCollection<PostTagDto> PostTags);
