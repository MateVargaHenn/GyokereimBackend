namespace Modules.Blog.Message;

internal sealed record UpdatePostCommand(
    int PostId,
    CreatePostDetailsDto PostDetailsDto)
    : ICommand;
