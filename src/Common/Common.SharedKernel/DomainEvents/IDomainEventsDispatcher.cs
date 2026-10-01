namespace Common.SharedKernel.DomainEvents;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents, 
        CancellationToken cancellationToken = default);
}
