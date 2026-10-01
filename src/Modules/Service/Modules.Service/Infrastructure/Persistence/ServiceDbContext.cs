using Common.SharedKernel.DomainEvents;

namespace Modules.Service.Infrastructure.Persistence;

internal sealed class ServiceDbContext(
    DbContextOptions<ServiceDbContext> options,
    IDomainEventsDispatcher? domainEventsDispatcher)
    : DbContext(options)
{
    private readonly IDomainEventsDispatcher? _domainEventsDispatcher = domainEventsDispatcher;
    internal DbSet<Domain.Service> Services => Set<Domain.Service>();
    internal DbSet<Domain.Order> Orders => Set<Domain.Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder
                    .ApplyConfigurationsFromAssembly(
                    typeof(ServiceDbContext).Assembly);

    public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .SelectMany(entry => entry.Entity.DomainEvents)
            .ToList();

        int result = await base.SaveChangesAsync(cancellationToken);

        if (domainEvents.Count != 0)
        {
            await _domainEventsDispatcher.DispatchAsync(
                domainEvents,
                cancellationToken);
        }

        return result;
    }
}
