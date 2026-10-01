using Modules.Service.Domain.Enum;

namespace Modules.Service.Domain;

internal class OrderStatus : EnumEntity<OrderStatusCode>
{
    internal IReadOnlyCollection<Order> Orders { get; } = [];
}
