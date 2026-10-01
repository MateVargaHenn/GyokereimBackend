using Common.Application.Pagination;

namespace Modules.Blog.Message;

internal sealed class GetPostsForAdminQueryHandler(
    BlogDbContext dbContext)
    : IQueryHandler<
        GetPostsForAdminQuery,
        PagedResult<AdminPostItemDto>>
{
    private const int PageSize = 10;

    public async Task<Result<PagedResult<AdminPostItemDto>>> Handle(
        GetPostsForAdminQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Post> query = dbContext.Posts
            .AsNoTracking();

        int totalCount =
            await query.CountAsync(cancellationToken);

        IReadOnlyCollection<AdminPostItemDto> posts =
            await query
                .OrderByDescending(x => x.UpdatedAt)
                .Skip((request.Page - 1) * PageSize)
                .Take(PageSize)
                .Select(x => new AdminPostItemDto(
                    x.Id,
                    x.Title,
                    x.Slug,
                    x.PostStatus,
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.PublishedAt))
                .ToListAsync(cancellationToken);

        var result = new PagedResult<AdminPostItemDto>(
            posts,
            request.Page,
            PageSize,
            totalCount);

        return Result<PagedResult<AdminPostItemDto>>
            .Success(result);
    }
}
