using Common.SharedKernel.Common;
using Common.SharedKernel.Messaging;

namespace Modules.File.Messaging;

internal sealed class UpdateFileMetadataCommandHandler(
    FileDbContext context)
    : ICommandHandler<UpdateFileMetadataCommand>
{
    public async Task<Result> Handle(
        UpdateFileMetadataCommand request,
        CancellationToken cancellationToken)
    {
        Domain.File? file =
            await context.Files
                .FirstOrDefaultAsync(
                    x => x.Id == request.FileId,
                    cancellationToken);

        if (file is null)
        {
            return Result.Failure(
                ErrorType.NotFound);
        }

        file.ChangeMetadata(
            request.Dto.Title,
            request.Dto.AltText);

        await context.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}
