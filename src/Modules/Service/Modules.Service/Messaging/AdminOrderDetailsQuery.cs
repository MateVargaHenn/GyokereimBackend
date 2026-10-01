namespace Modules.Service.Messaging;

internal sealed record AdminOrderDetailsQuery(
    int OrderId)
    : IQuery<AdminOrderDetailsDto>;
