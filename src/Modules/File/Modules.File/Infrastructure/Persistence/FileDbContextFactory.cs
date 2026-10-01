using Common.SharedKernel.DomainEvents;

namespace Modules.File.Infrastructure.Persistence;

internal sealed class FileDbContextFactory
    : DesignTimeDbContextFactory<FileDbContext>
{
    protected override string Schema => Schemas.File;

    protected override FileDbContext CreateContext(
        DbContextOptions<FileDbContext> options, IDomainEventsDispatcher? domainEventsDispatcher)

        => new(options, domainEventsDispatcher!);
}
