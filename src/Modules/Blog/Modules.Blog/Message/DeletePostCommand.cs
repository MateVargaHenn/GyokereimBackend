namespace Modules.Blog.Message;

internal sealed record DeletePostCommand(int PostId)
    : ICommand;
