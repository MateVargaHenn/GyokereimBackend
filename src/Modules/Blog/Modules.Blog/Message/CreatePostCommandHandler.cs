namespace Modules.Blog.Message;

internal sealed class CreatePostCommandHandler(
    BlogDbContext dbContext)
    : ICommandHandler<CreatePostCommand>
{
    public async Task<Result> Handle(
        CreatePostCommand command,
        CancellationToken cancellationToken)
    {
        CreatePostDetailsDto dto =
            command.CreatePostDetailsDto;

        var post = Post.Create(
            command.AuthorId,
            dto.Title,
            dto.MetaTitle,
            dto.Slug,
            dto.Summary,
            dto.Content);

        foreach (PostMetaDto meta in dto.PostMetas)
        {
            post.AddMeta(
                meta.Key,
                meta.Value);
        }

        foreach (PostTagDto tagDto in dto.PostTags)
        {
            Tag? tag = await dbContext.Tags
                .FirstOrDefaultAsync(
                    x => x.Slug == tagDto.Slug,
                    cancellationToken);

            if (tag is null)
            {
                tag = Tag.Create(
                    tagDto.Name,
                    tagDto.Slug);

                dbContext.Tags.Add(tag);
            }

            post.AddTag(tag);
        }

        dbContext.Posts.Add(post);

        await dbContext.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}
