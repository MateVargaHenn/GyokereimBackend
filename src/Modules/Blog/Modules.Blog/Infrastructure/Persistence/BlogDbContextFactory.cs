using Common.SharedKernel.DomainEvents;

namespace Modules.Contact.Infrastructure.Persistence;

internal sealed class BlogDbContextFactory
    : DesignTimeDbContextFactory<BlogDbContext>
{
    protected override string Schema => Schemas.Blog;

    protected override BlogDbContext CreateContext(
        DbContextOptions<BlogDbContext> options, IDomainEventsDispatcher? domainEventsDispatcher)

        => new(options, domainEventsDispatcher);
}
