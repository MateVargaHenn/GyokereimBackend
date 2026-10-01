using Common.SharedKernel.DomainEvents;

namespace Modules.File.Infrastructure.Persistence;

internal class FileDbContext : DbContext
{
    private readonly IDomainEventsDispatcher _domainEventsDispatcher;

    public FileDbContext(
    DbContextOptions<FileDbContext> options,
    IDomainEventsDispatcher domainEventsDispatcher)
    : base(options) => _domainEventsDispatcher = domainEventsDispatcher;

    public DbSet<Domain.File> Files => Set<Domain.File>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.File);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileDbContext).Assembly);

    }
}
