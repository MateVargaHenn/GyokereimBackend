using Common.SharedKernel.DomainEvents;

namespace Modules.Chat.Infrastructure.Persistence;

internal sealed class ChatDbContext(
    DbContextOptions<ChatDbContext> options, IDomainEventsDispatcher? domainEventsDispatcher)
    : DbContext(options)
{
    private readonly IDomainEventsDispatcher? _domainEventsDispatcher = domainEventsDispatcher;
    internal DbSet<Intent> Intents => Set<Intent>();

    internal DbSet<Answer> Answers => Set<Answer>();

    internal DbSet<Utterance> Utterances => Set<Utterance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) => 
        modelBuilder
                    .ApplyConfigurationsFromAssembly(
                    typeof(ChatDbContext).Assembly);
}
