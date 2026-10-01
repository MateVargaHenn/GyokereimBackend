

namespace Modules.File.Infrastructure;

internal sealed class UploadFileModule(
    FileDbContext dbContext,
    IFileStorage fileStorage,
    ILogger<UploadFileModule> logger)
    : IFileModule
{
    private readonly string[] enabledExtensions =
    [
        ".jpg",
        ".jpeg",
        ".png",
        ".pdf",
        ".gif",
        ".docx",
        ".xls",
        ".xlsx",
        ".csv",
        ".ppt",
        ".pptx"
    ];

    public async Task<int> SaveFileAsync(
        FileUpload file,
        CancellationToken cancellationToken = default)
    {
        string extension =
            Path.GetExtension(file.FileName);

        if (!enabledExtensions.Contains(
            extension,
            StringComparer.OrdinalIgnoreCase))
        {
            throw new DisabledFileExtensionException(
                $"A fájl kiterjesztés '{extension}' nem engedélyezett.");
        }

        string key =
            Guid.NewGuid()
                .ToString("N");

        await using var contentStream =
            new MemoryStream(
                file.Content,
                writable: false);

        string storagePath =
            await fileStorage.SaveAsync(
                key,
                extension,
                contentStream,
                cancellationToken);

        var entity =
            Domain.File.Create(
                key,
                file.FileName,
                file.ContentType,
                file.Content.LongLength,
                storagePath);

        try
        {
            dbContext.Files.Add(entity);

            await dbContext.SaveChangesAsync(
                cancellationToken);

            return entity.Id;
        }
        catch
        {
            dbContext.Entry(entity).State =
                EntityState.Detached;

            try
            {
                await fileStorage.DeleteAsync(
                    storagePath,
                    CancellationToken.None);
            }
            catch (Exception cleanupException)
            {
                logger.LogError(
                    cleanupException,
                    "Nem sikerült törölni az árva fájlt a storage-ból: {StoragePath}",
                    storagePath);
            }

            throw;
        }
    }
}
