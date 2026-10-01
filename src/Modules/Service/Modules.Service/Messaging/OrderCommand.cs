namespace Modules.Service.Messaging;

internal class OrderCommand(OrderDto orderDto) : ICommand
{
    internal OrderDto OrderDto { get; private set; } = orderDto;
}
