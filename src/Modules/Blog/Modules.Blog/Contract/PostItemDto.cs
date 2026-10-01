namespace Modules.Blog.Contract;

public sealed record PostItemDto(
    int Id,
    string Title,
    string Slug,
    string Summary,
    DateTime? PublishedAt);
