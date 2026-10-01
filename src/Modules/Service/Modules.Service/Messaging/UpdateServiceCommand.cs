namespace Modules.Service.Messaging;

internal sealed record UpdateServiceCommand(
    int ServiceId,
    CreateServiceDto ServiceDto)
    : ICommand;
