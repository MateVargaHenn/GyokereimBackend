namespace Modules.Service.Messaging;

internal sealed record ActivateServiceCommand(
    int ServiceId)
    : ICommand;
