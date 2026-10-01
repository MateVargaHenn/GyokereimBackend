namespace Modules.Contact.Infrastructure.Persistence;

internal sealed class ContactDbContextFactory
    : DesignTimeDbContextFactory<ContactDbContext>
{
    protected override string Schema => Schemas.Contact;

    protected override ContactDbContext CreateContext(
        DbContextOptions<ContactDbContext> options, IDomainEventsDispatcher? domainEventsDispatcher)

        => new(options, domainEventsDispatcher);
}
