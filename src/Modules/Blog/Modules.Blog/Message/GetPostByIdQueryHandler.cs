namespace Modules.Blog.Message;

internal sealed class GetPostByIdQueryHandler(
    BlogDbContext dbContext)
    : IQueryHandler<GetPostByIdQuery, PostDetailsDto>
{
    public async Task<Result<PostDetailsDto>> Handle(
        GetPostByIdQuery request,
        CancellationToken cancellationToken)
    {
        PostDetailsDto? post = await dbContext.Posts
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
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

        if (post is null)
        {
            return Result.Failure<PostDetailsDto>(Error.NotFound);
        }

        return Result<PostDetailsDto>
            .Success(post);
    }
}
