namespace Modules.Blog.Message;

internal record GetPostBySlugQuery(string Slug)
    : IQuery<PostDetailsDto>;
