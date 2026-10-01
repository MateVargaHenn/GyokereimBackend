namespace Modules.Blog.Message;

internal sealed record CreatePostCommand(
    string AuthorId,
    CreatePostDetailsDto CreatePostDetailsDto)
    : ICommand;
