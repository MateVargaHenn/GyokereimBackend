using Common.SharedKernel.DomainEvents;

namespace Modules.Blog.Infrastructure.Persistence;

internal sealed class BlogDbContext(
    DbContextOptions<BlogDbContext> options,
    IDomainEventsDispatcher domainEventsDispatcher)
    : DbContext(options)
{
    private readonly IDomainEventsDispatcher _domainEventsDispatcher = domainEventsDispatcher;

    internal DbSet<Post> Posts => Set<Post>();
    internal DbSet<Tag> Tags => Set<Tag>();
    internal DbSet<PostTag> PostTags => Set<PostTag>();
    internal DbSet<PostMeta> PostMetas => Set<PostMeta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Blog);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BlogDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
                .Entries<Entity>()
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

        int result = await base.SaveChangesAsync(cancellationToken);

        await domainEventsDispatcher.DispatchAsync(
            domainEvents,
            cancellationToken);

        return result;
    }
}
