namespace Modules.Blog.Contract;

public sealed record AdminPostItemDto(
    int Id,
    string Title,
    string Slug,
    PostStatus PostStatus,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? PublishedAt);
