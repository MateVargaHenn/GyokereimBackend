

namespace Modules.Blog.Message;

public sealed record GetPublishedPostsForHomeQuery
    : IQuery<IReadOnlyCollection<PostItemDto>>;
