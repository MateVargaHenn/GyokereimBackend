using Common.SharedKernel.DomainEvents;

namespace Modules.Chat.Infrastructure.Persistence;


internal sealed class ChatDbContextFactory
    : DesignTimeDbContextFactory<ChatDbContext>
{
    protected override string Schema => Schemas.Chat;

    protected override ChatDbContext CreateContext(
        DbContextOptions<ChatDbContext> options,
        IDomainEventsDispatcher? domainEventsDispatcher)
        => new(options, domainEventsDispatcher);
}
