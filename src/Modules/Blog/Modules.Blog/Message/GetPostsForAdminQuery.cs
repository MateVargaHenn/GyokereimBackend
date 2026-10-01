using Common.Application.Pagination;

namespace Modules.Blog.Message;

internal sealed record GetPostsForAdminQuery(
    int Page)
    : IQuery<PagedResult<AdminPostItemDto>>;
