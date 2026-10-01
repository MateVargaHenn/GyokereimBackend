namespace Modules.Service.Domain;

internal sealed class Order : Entity
{
    internal string CustomerName { get; private set; } = string.Empty;
    internal string Email { get; private set; } = string.Empty;
    internal string PhoneNumber { get; private set; } = string.Empty;
    internal string? Message { get; private set; }
    internal OrderStatusCode OrderStatusCode { get; private set; }
    internal OrderStatus OrderStatus { get; private set; } = default!;
    internal int ServiceId { get; private set; }
    internal Domain.Service Service { get; private set; } = default!;
    internal string TrackingCode { get; private set; } = string.Empty; 
    internal DateTime Created { get; private set; }

    public static Order Create(
        string customerName,
        string email,
        string phoneNumber,
        string? message,
        int serviceId)
    {
        var order = new Order
        {
            CustomerName = customerName,
            Email = email,
            PhoneNumber = phoneNumber,
            Message = message,
            ServiceId = serviceId,

            TrackingCode = GenerateTrackingCode(),

            OrderStatusCode = OrderStatusCode.Created,
            Created = DateTime.UtcNow
        };

        order.RaiseDomainEvent(
            new OrderPlacedDomainEvent(order));

        return order;
    }

    private static string GenerateTrackingCode() => Guid.NewGuid()
            .ToString("N")
            .ToUpperInvariant();

    public void ChangeStatus(OrderStatusCode newStatus)
    {
        if (OrderStatusCode == newStatus)
        {
            return;
        }
        OrderStatusCode = newStatus;
        RaiseDomainEvent(
            new OrderStatusChangedDomainEvent(this));
    }
}
