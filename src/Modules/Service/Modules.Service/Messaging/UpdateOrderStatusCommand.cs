namespace Modules.Service.Messaging;

internal sealed record UpdateOrderStatusCommand(
    int OrderId,
    OrderStatusCode OrderStatusCode)
    : ICommand;
