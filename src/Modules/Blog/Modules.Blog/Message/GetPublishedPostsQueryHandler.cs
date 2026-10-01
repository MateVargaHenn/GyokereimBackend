using Common.Application.Pagination;

namespace Modules.Blog.Message;

internal sealed class GetPublishedPostsQueryHandler(
    BlogDbContext dbContext)
    : IQueryHandler<
        GetPublishedPostsQuery,
        PagedResult<PostItemDto>>
{
    public async Task<Result<PagedResult<PostItemDto>>> Handle(
        GetPublishedPostsQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Post> query = dbContext.Posts
            .AsNoTracking()
            .Where(x => x.PostStatus == PostStatus.Published);

        int totalCount = await query.CountAsync(cancellationToken);

        IReadOnlyCollection<PostItemDto> posts = await query
            .OrderByDescending(x => x.PublishedAt)
            .Skip((request.Page - 1) * 3)
            .Take(3)
            .Select(x => new PostItemDto(
                x.Id,
                x.Title,
                x.Slug,
                x.Summary,
                x.PublishedAt))
            .ToListAsync(cancellationToken);

        var result = new PagedResult<PostItemDto>(
            posts,
            request.Page,
            3,
            totalCount);

        return Result<PagedResult<PostItemDto>>
            .Success(result);
    }
}
