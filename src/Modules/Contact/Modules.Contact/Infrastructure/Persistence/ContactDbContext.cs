namespace Modules.Contact.Infrastructure.Persistence;

public sealed class ContactDbContext(
    DbContextOptions<ContactDbContext> options,
    IDomainEventsDispatcher? domainEventsDispatcher)
    : DbContext(options)
{
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Contact);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ContactDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
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
