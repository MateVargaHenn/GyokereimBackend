namespace Modules.Blog.Message;

internal sealed class PublishPostCommandHandler(
    BlogDbContext dbContext)
    : ICommandHandler<PublishPostCommand>
{
    public async Task<Result> Handle(
        PublishPostCommand command,
        CancellationToken cancellationToken)
    {
        Post? post = await dbContext.Posts
            .FirstOrDefaultAsync(
                x => x.Id == command.PostId,
                cancellationToken);

        if (post is null)
        {
            return Result.Failure(ErrorType.NotFound);
        }

        post.Publish();

        await dbContext.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}
