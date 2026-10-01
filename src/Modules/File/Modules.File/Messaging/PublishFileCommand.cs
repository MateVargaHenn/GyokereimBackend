using Common.SharedKernel.Messaging;

namespace Modules.File.Messaging;

internal sealed record PublishFileCommand(
    int FileId)
    : ICommand;
