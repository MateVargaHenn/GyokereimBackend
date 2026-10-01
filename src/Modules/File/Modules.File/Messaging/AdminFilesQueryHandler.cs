using Common.SharedKernel.Common;
using Common.SharedKernel.Messaging;

namespace Modules.File.Messaging;

internal sealed class AdminFilesQueryHandler(
    FileDbContext context)
    : IQueryHandler<
        AdminFilesQuery,
        List<AdminFileItemDto>>
{
    public async Task<Result<List<AdminFileItemDto>>> Handle(
        AdminFilesQuery request,
        CancellationToken cancellationToken)
    {
        List<AdminFileItemDto> files =
            await context.Files
                .AsNoTracking()
                .OrderByDescending(x => x.LastModified)
                .Select(x =>
                    new AdminFileItemDto(
                        x.Id,
                        x.Key,
                        x.FileName,
                        x.ContentType,
                        x.Size,
                        x.Title,
                        x.AltText,
                        x.IsPublic,
                        x.LastModified))
                .ToListAsync(cancellationToken);

        return Result.Success(files);
    }
}
