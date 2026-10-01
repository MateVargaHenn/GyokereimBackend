namespace Modules.Blog.Message;

internal sealed class UpdatePostCommandHandler(
    BlogDbContext dbContext)
    : ICommandHandler<UpdatePostCommand>
{
    public async Task<Result> Handle(
        UpdatePostCommand command,
        CancellationToken cancellationToken)
    {
        Post? post = await dbContext.Posts
            .Include(x => x.Meta)
            .Include(x => x.PostTags)
            .FirstOrDefaultAsync(
                x => x.Id == command.PostId,
                cancellationToken);

        if (post is null)
        {
            return Result.Failure(ErrorType.NotFound);
        }

        CreatePostDetailsDto dto =
            command.PostDetailsDto;

        post.ChangeTitle(dto.Title);
        post.ChangeMetaTitle(dto.MetaTitle);
        post.ChangeSlug(dto.Slug);
        post.ChangeSummary(dto.Summary);
        post.ChangeContent(dto.Content);

        UpdateMetas(
            post,
            dto.PostMetas);

        await UpdateTagsAsync(
            post,
            dto.PostTags,
            cancellationToken);

        await dbContext.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }

    private static void UpdateMetas(
        Post post,
        IReadOnlyCollection<PostMetaDto> metas)
    {
        var incomingKeys = metas
            .Select(x => x.Key)
            .ToHashSet(
                StringComparer.OrdinalIgnoreCase);

        string[] removedKeys = post.Meta
            .Where(x => !incomingKeys.Contains(x.Key))
            .Select(x => x.Key)
            .ToArray();

        foreach (string key in removedKeys)
        {
            post.RemoveMeta(key);
        }

        foreach (PostMetaDto meta in metas)
        {
            post.AddMeta(
                meta.Key,
                meta.Value);
        }
    }

    private async Task UpdateTagsAsync(
        Post post,
        IReadOnlyCollection<PostTagDto> tags,
        CancellationToken cancellationToken)
    {
        var incomingSlugs = tags
            .Select(x => x.Slug)
            .ToHashSet(
                StringComparer.OrdinalIgnoreCase);

        int[] currentTagIds = post.PostTags
            .Select(x => x.TagId)
            .ToArray();

        List<Tag> currentTags = await dbContext.Tags
            .Where(x => currentTagIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (Tag tag in currentTags)
        {
            if (!incomingSlugs.Contains(tag.Slug))
            {
                post.RemoveTag(tag);
            }
        }

        foreach (PostTagDto tagDto in tags)
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
    }
}
