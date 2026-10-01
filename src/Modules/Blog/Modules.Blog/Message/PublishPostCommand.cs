namespace Modules.Blog.Message;

internal sealed record PublishPostCommand(int PostId)
    : ICommand;
