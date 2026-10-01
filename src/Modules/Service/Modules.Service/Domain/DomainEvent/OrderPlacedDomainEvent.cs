namespace Modules.Service.Domain.DomainEvent;

internal sealed record OrderPlacedDomainEvent(
    Order Order) : IDomainEvent;
