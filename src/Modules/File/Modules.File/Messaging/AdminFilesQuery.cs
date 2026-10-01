using Common.SharedKernel.Messaging;

namespace Modules.File.Messaging;

internal sealed record AdminFilesQuery
    : IQuery<List<AdminFileItemDto>>;
