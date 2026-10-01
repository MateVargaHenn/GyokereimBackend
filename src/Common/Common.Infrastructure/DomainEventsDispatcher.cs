namespace Common.Infrastructure;

public sealed class DomainEventsDispatcher(
    IPublisher publisher)
    : IDomainEventsDispatcher
{
    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(
                domainEvent,
                cancellationToken);
        }
    }
}
