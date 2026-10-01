namespace Modules.Blog.Message;

internal sealed class MovePostToDraftCommandHandler(
    BlogDbContext dbContext)
    : ICommandHandler<MovePostToDraftCommand>
{
    public async Task<Result> Handle(
        MovePostToDraftCommand command,
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

        post.MoveToDraft();

        await dbContext.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}
