using Common.SharedKernel.Common;
using Common.SharedKernel.Messaging;

namespace Modules.File.Messaging;

internal sealed class PublishFileCommandHandler(
    FileDbContext context)
    : ICommandHandler<PublishFileCommand>
{
    public async Task<Result> Handle(
        PublishFileCommand request,
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

        file.Publish();

        await context.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}
