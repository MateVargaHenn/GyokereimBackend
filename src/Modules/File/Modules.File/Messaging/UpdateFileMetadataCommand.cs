using Common.SharedKernel.Messaging;

namespace Modules.File.Messaging;

internal sealed record UpdateFileMetadataCommand(
    int FileId,
    UpdateFileMetadataDto Dto)
    : ICommand;
