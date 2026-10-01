namespace Modules.Blog.Message;

internal class GetPostBySlugQueryHandler(
    BlogDbContext dbContext)
    : IQueryHandler<GetPostBySlugQuery, PostDetailsDto>
{
    public async Task<Result<PostDetailsDto>> Handle(GetPostBySlugQuery request, CancellationToken cancellationToken)
    {
        PostDetailsDto? postItemDto = await dbContext.Posts
            .AsNoTracking()
            .Where(x =>
                x.PostStatus == PostStatus.Published &&
                x.Slug == request.Slug)
            .Select(post => new PostDetailsDto(
                post.Id,
                post.AuthorId,
                post.Title,
                post.MetaTitle,
                post.Slug,
                post.Summary,
                post.Content,
                post.PostStatus,
                post.PublishedAt,
                post.Meta
                    .Select(meta =>
                        new PostMetaDto(
                            meta.Key,
                            meta.Value))
                    .ToList(),
                post.PostTags
                    .Select(postTag =>
                        new PostTagDto(
                            postTag.Tag.Name,
                            postTag.Tag.Slug))
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (postItemDto is null)
        {
            return Result.Failure<PostDetailsDto>(Error.NotFound);
        }

        return Result<PostDetailsDto>.Success(postItemDto);
    }
}
