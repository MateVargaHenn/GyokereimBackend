namespace Modules.Service.Messaging;

internal sealed record CreateServiceCommand(
    CreateServiceDto ServiceDto)
    : ICommand;
