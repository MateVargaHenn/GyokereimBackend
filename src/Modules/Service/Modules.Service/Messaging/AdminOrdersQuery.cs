namespace Modules.Service.Messaging;

internal sealed record AdminOrdersQuery
    : IQuery<List<AdminOrderItemDto>>;
