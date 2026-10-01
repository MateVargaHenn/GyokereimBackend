namespace Modules.Blog.Message;

internal sealed record GetPostByIdQuery(int Id)
    : IQuery<PostDetailsDto>;
