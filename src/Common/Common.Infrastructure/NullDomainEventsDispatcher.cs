namespace Common.Infrastructure;

public sealed class NullDomainEventsDispatcher
    : IDomainEventsDispatcher
{
    public static readonly NullDomainEventsDispatcher Instance = new();

    private NullDomainEventsDispatcher()
    {
    }

    public Task DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
