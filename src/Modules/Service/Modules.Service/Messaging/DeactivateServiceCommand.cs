namespace Modules.Service.Messaging;

internal sealed record DeactivateServiceCommand(
    int ServiceId)
    : ICommand;
