namespace Modules.Service.Domain.DomainEvent;

internal sealed record OrderStatusChangedDomainEvent(
    Order Order) : IDomainEvent;
