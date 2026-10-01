namespace Modules.Blog.Message;

internal sealed class DeletePostCommandHandler(
    BlogDbContext dbContext)
    : ICommandHandler<DeletePostCommand>
{
    public async Task<Result> Handle(
        DeletePostCommand command,
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

        dbContext.PostMetas.RemoveRange(post.Meta);
        dbContext.PostTags.RemoveRange(post.PostTags);
        dbContext.Posts.Remove(post);

        await dbContext.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}
