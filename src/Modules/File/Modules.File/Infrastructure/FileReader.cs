namespace Modules.File.Infrastructure;

internal sealed class FileReader(FileDbContext dbContext,
    IFileStorage fileStorage)
    : IFileReader
{
    public async Task<FileData?> GetFileAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        Domain.File? file = await dbContext.Files
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Key == key,
                cancellationToken);

        if (file is null)
        {
            return null;
        }

        Stream content = await fileStorage.GetAsync(
            file.StoragePath,
            cancellationToken);

        return new FileData(
            file.FileName,
            file.ContentType,
            content);
    }

    public async Task<FileData?> GetFileAsync(int id, CancellationToken cancellationToken = default)
    {
        Domain.File? file = await dbContext.Files
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (file is null)
        {
            return null;
        }

        Stream content = await fileStorage.GetAsync(
            file.StoragePath,
            cancellationToken);

        return new FileData(
            file.FileName,
            file.ContentType,
            content);

    }
}
