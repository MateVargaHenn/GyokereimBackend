namespace Modules.Blog.Message;

internal sealed class GetPublishedPostsForHomeQueryHandler(
    BlogDbContext dbContext)
    : IQueryHandler<
        GetPublishedPostsForHomeQuery,
        IReadOnlyCollection<PostItemDto>>
{
    public async Task<Result<IReadOnlyCollection<PostItemDto>>> Handle(
        GetPublishedPostsForHomeQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyCollection<PostItemDto> posts = await dbContext.Posts
            .AsNoTracking()
            .Where(x => x.PostStatus == PostStatus.Published)
            .OrderByDescending(x => x.PublishedAt)
            .Take(3)
            .Select(x => new PostItemDto(
                x.Id,
                x.Title,
                x.Slug,
                x.Summary,
                x.PublishedAt))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<PostItemDto>>
            .Success(posts);
    }
}
