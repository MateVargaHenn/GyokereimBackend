using Common.SharedKernel.Common;
using Common.SharedKernel.Messaging;

namespace Modules.File.Messaging;

internal sealed record UnpublishFileCommand(
    int FileId)
    : ICommand;
