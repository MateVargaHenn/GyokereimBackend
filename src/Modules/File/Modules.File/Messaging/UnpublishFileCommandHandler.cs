using Common.SharedKernel.Common;
using Common.SharedKernel.Messaging;

namespace Modules.File.Messaging;

internal sealed class UnpublishFileCommandHandler(
    FileDbContext context)
    : ICommandHandler<UnpublishFileCommand>
{
    public async Task<Result> Handle(
        UnpublishFileCommand request,
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

        file.Unpublish();

        await context.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}
