using Common.Application.Pagination;

namespace Modules.Blog.Message;

internal sealed record GetPublishedPostsQuery(
    int Page = 1)
    : IQuery<PagedResult<PostItemDto>>;
