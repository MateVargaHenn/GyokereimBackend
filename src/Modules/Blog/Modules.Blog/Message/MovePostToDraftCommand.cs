namespace Modules.Blog.Message;

internal sealed record MovePostToDraftCommand(int PostId)
    : ICommand;
